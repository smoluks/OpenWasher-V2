#include <stdbool.h>
#include <stdio.h>
#include "stm32f10x.h"
#include "engine_driver.h"
#include "engine_hardware.h"
#include "eeprom.h"
#include "systick.h"
#include "delay.h"
#include "error.h"
#include "pid.h"
#include "phaseshifting.h"

extern volatile bool ct;

volatile enum engine_feedback_state_e engine_feedback = ef_off;
//---rotate per second * 16 (encoder tick per second)---
volatile uint16_t engine_current_speed = 0;
volatile uint16_t engine_target_speed = 0;
//
volatile uint16_t engine_regulvalue = 0; //VALUE_FULL max
volatile bool pid_enable = false;
volatile uint16_t tacho_adcvalue = 0;

bool engine_test()
{
	printf("Test engine\nCheck half feedback\n");

	//check feedback
	uint32_t timestamp = get_systime();
	while (engine_feedback != ef_half && checkdelay(timestamp, 1000));
	if (engine_feedback != ef_half)
	{
		printf("Engine test need closed door\n");
		return false;
	}

	//check feedback with relay on
	printf("Half feedback OK\nCheck relay\n");
	pid_enable = false;
	engine_regulvalue = 0;
	delay_ms(11u);

	engine_setdirectionccw();

	getsystime(&timestamp);
	while (engine_feedback != ef_full && checkdelay(timestamp, 1000));
	if(engine_feedback != ef_full)
	{
		set_error(NO_ENGINE);
		engine_setdirectionoff();
		return false;
	}
	printf("Relay feedback present at %lu ms\nCheck tacho noise...\n", delta(timestamp)+100u);

	//check noise on tacho
	getsystime(&timestamp);
	uint16_t maxtachovalue = 0;
	while (!ct && delta(timestamp) < 15000)
	{
		if(tacho_adcvalue > maxtachovalue)
			maxtachovalue = tacho_adcvalue;
	}
	if (ct)
	{
		engine_setdirectionoff();
		return false;
	}
	eeprom_config.tachodetectlevel = maxtachovalue+10;
	printf("Tacho noise level %u\nWait start rotating...\n", maxtachovalue);

	//find start value
	while (engine_current_speed == 0 && engine_regulvalue < VALUE_FULL && !ct)
	{
		engine_regulvalue++;
		delay_ms(50u);
	}
	if (!ct)
	{
		if (engine_regulvalue < VALUE_FULL)
		{
			eeprom_config.enginestartvalue = engine_regulvalue < 400 ? 0 : engine_regulvalue - 400;
			printf("Start at value %u\n", engine_regulvalue);
		}
		else set_error(NO_TACHO);
	}

	while (engine_regulvalue > 0)
	{
		engine_regulvalue--;
		delay_ms(2u);
	}

	//check feedback
	printf("Check relay disable feedback\n");
	engine_setdirectionoff();

	getsystime(&timestamp);
	while (engine_feedback != ef_half && delta(timestamp) < 1000u);
	if(engine_feedback != ef_half)
	{
		set_error(ENGINE_RELAYCW_STICKING);
		return false;
	}
	printf("Disable feedback present at %lu ms\nCheck PID\n", delta(timestamp)+11u);

	//check PID
	//TODO: measure fluctuations/autocalibrate PID
	engine_settargetrps(10, cw);

	getsystime(&timestamp);
	while (engine_getcurrentrps() < 10 && !ct && checkdelay(timestamp, 15000u));
	if(engine_getcurrentrps() < 10)
	{
		set_error(PID_ERROR);
		engine_settargetrps(0, off);
		return false;
	}
	uint32_t accstime = delta(timestamp);

	getsystime(&timestamp);
	while(!ct && checkdelay(timestamp, 10000u))
	{
		if(engine_getcurrentrps() != 10)
		{
			//set_error(PID_ERROR);
			//engine_settargetrps(0, off);
			//return false;
		}
	}

	if(ct)
	{
		engine_settargetrps(0, off);
		return false;
	}

	engine_settargetrps(0, off);
	printf("Take 10 ppm at %lu ms\nTest engine OK\n", accstime);

	return true;
}

bool engine_calibratepid()
{
	pid_enable = false;
	engine_regulvalue = 0;
	delay_ms(11u);

	engine_setdirectionccw();

	while (engine_current_speed == 0 && engine_regulvalue < VALUE_FULL && !ct) {
		engine_regulvalue++;
		delay_ms_with_ct(10000u);
	}
	if (!ct) {
		if (engine_regulvalue < VALUE_FULL) {
			printf("Start at value %u and speed %u\n", engine_regulvalue, engine_current_speed);
		} else
			set_error(NO_TACHO);
	}

	for (int i = 2; i <= 10; i++) {
		while (engine_getcurrentrps() < i && engine_regulvalue < VALUE_FULL && !ct) {
			engine_regulvalue++;
			delay_ms_with_ct(10000u);
		}
		if (ct)
			break;

		if (engine_regulvalue < VALUE_FULL) {
			printf("Set speed %u at value %u\n", i, engine_regulvalue);
		} else {
			set_error(NO_TACHO);
			break;
		}
	}

	engine_setdirectionoff();

	return !ct;
}

//relay control
bool engine_setdirection(enum direction_e direction)
{
	engine_regulvalue = 0;
	pid_enable = false;

	enum direction_e dir = get_direction();
	if(dir == direction)
		return true;

	if(direction == off)
	{
		engine_setdirectionoff();

		uint32_t timestamp = get_systime();
		while (engine_feedback == ef_full && checkdelay(timestamp, 2000u));
		if (engine_feedback == ef_full)
		{
			if(dir == cw)
				set_error(ENGINE_RELAYCW_STICKING);
			else
				set_error(ENGINE_RELAYCCW_STICKING);
			return false;
		}
	}
	else
	{
		if(direction == cw)
			engine_setdirectioncw();
		else
			engine_setdirectionccw();

		uint32_t timestamp = get_systime();
		while (engine_feedback != ef_full && checkdelay(timestamp, 2000u));
		if (engine_feedback != ef_full)
		{
			set_error(NO_ENGINE);
			engine_setdirectionoff();
			return false;
		}
	}

	return true;
}

uint8_t engine_getcurrentrps()
{
	return engine_current_speed >> 4;
}

bool engine_settargetrps(uint8_t rps, enum direction_e direction)
{
	pid_enable = false;
	enum direction_e dir = get_direction();

	//if target 0 or direction not current direction, stop
	if ((rps == 0 || direction != dir) && dir != off)
	{
		pid_enable = false;
		engine_regulvalue = 0;
		engine_target_speed = 0;
		delay_ms(20u);

		uint32_t timestamp = get_systime();
		while (engine_current_speed > 0 && checkdelay(timestamp, 10000u));
		if(engine_current_speed > 0)
		{
			set_error(ENGINE_TRIAK_STICKING);
			return false;
		}

		//relay off
		return engine_setdirection(off);
	}

	//set new value
	if(direction != off && rps > 0)
	{
		if(dir == off)
		{
			//---start if needed---
			pid_clearstate();

			if(!engine_setdirection(direction))
				return false;
		}

		engine_target_speed = rps << 4;
		pid_enable = true;
	}

	return !ct;
}

inline void engine_crosszero() {
	engine_triakoff();

	//stop timers
	TIM2->CR1 &= ~TIM_CR1_CEN;
	TIM2->CNT = 0;
	TIM4->CR1 &= ~TIM_CR1_CEN;
	TIM4->CNT = 0;

	//calculate PID
	if (pid_enable)
		engine_regulvalue = pid_process((int32_t)engine_current_speed, (int32_t)engine_target_speed);

	if (engine_regulvalue >= VALUE_FULL)
	{
		engine_triakon();
	} else if (engine_regulvalue > 0)
	{
		//start on timer
		TIM4->SR &= ~TIM_SR_UIF;
		TIM4->ARR = phaseTable[engine_regulvalue];
		TIM4->CR1 |= TIM_CR1_CEN;
	}
}

volatile bool pulse = false;
inline void process_tacho(uint16_t value)
{
	tacho_adcvalue = value;
	//front detector
	if(!pulse && value >= eeprom_config.tachodetectlevel + 10)
	{
		pulse = true;

		if(TIM3->CR1 & TIM_CR1_CEN)
		{
			//calc rps
			if(TIM3->CNT != 0)
				engine_current_speed = 100000 / TIM3->CNT;
		}
		else
			engine_current_speed = 0;

		//start timer
		TIM3->CR1 &= ~TIM_CR1_CEN;
		TIM3->CNT = 0;
		TIM3->CR1 |= TIM_CR1_CEN;
	}
	else if(pulse && value < eeprom_config.tachodetectlevel)
	{
		pulse = false;
	}
}

inline void process_feedback(enum engine_feedback_state_e current)
{
	engine_feedback = current;
}

extern uint32_t engine_feedback_timestamp;
inline void engine_systick()
{
	if (engine_feedback && !checkdelay(engine_feedback_timestamp, 15U))
	{
		engine_feedback = ef_off;
	}
}

void engine_emergencystop()
{
	engine_triakoff();
	engine_setdirectionoff();
	pid_enable = false;
	engine_regulvalue = 0;
}

inline enum engine_feedback_state_e get_engine_feedback()
{
	return engine_feedback;
}

