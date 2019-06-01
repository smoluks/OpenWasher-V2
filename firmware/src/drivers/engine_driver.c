#include <delay.h>
#include <eeprom.h>
#include <engine_driver.h>
#include <error.h>
#include <phaseshifting.h>
#include <stdint-gcc.h>
#include <stdio.h>
#include <hardware.h>
#include <systick.h>
#include "engine_hardware.h"

bool engine_setdirection(enum direction_e direction);
void process_feedback(enum enginedlfeedbackstate current);

extern volatile bool ct;

volatile enum enginedlfeedbackstate engine_feedback = ef_off;
volatile uint8_t tacho_currentrps = 0;
volatile uint8_t tacho_targetrps = 0;
volatile int16_t engine_regulvalue = 0; //VALUE_FULL max
volatile bool pid_enable = false;
volatile uint16_t tacho_rawvalue = 0;

bool engine_test()
{
	printf("Test engine\nCheck half feedback\n");

	uint32_t timestamp;
	getsystime(&timestamp);
	while (engine_feedback != ef_half && checkdelay(timestamp, 1000));
	if (engine_feedback != ef_half)
	{
		printf("Engine test need closed door\n");
		return false;
	}

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
	printf("Relay feedback present at %u ms\nCheck tacho noise...\n", delta(timestamp)+100u);

	getsystime(&timestamp);
	uint16_t maxtachovalue = 0;
	while (!ct && delta(timestamp) < 15000)
	{
		if(tacho_rawvalue > maxtachovalue)
			maxtachovalue = tacho_rawvalue;
	}
	if (ct)
	{
		engine_setdirectionoff();
		return false;
	}
	eeprom_config.tachodetectlevel = maxtachovalue+10;
	printf("Tacho noise level %u\nWait start rotating...\n", maxtachovalue);

	while (tacho_currentrps == 0 && engine_regulvalue < VALUE_FULL && !ct)
	{
		engine_regulvalue++;
		delay_ms(50u);
	}
	if (!ct)
	{
		if (engine_regulvalue < VALUE_FULL)
		{
			eeprom_config.enginestartvalue = engine_regulvalue < 100 ? 0 : engine_regulvalue - 400;
			printf("Start at value %u\n", engine_regulvalue);
		}
		else set_error(NO_TACHO);
	}

	while (engine_regulvalue > 0)
	{
		engine_regulvalue--;
		delay_ms(2u);
	}

	printf("Check relay disable feedback\n");
	engine_setdirectionoff();

	getsystime(&timestamp);
	while (engine_feedback != ef_half && delta(timestamp) < 1000u);
	if(engine_feedback != ef_half)
	{
		set_error(ENGINE_RELAYCW_STICKING);
		return false;
	}
	printf("Disable feedback present at %u ms\nCheck PID\n", delta(timestamp)+11u);

	//
	engine_settargetrps(10, cw);
	getsystime(&timestamp);
	while (tacho_currentrps < 10 && !ct && checkdelay(timestamp, 15000u));
	if(ct)
	{
		engine_settargetrps(0, off);
		return false;
	}
	if(tacho_currentrps < 10)
	{
		set_error(PID_ERROR);
		engine_settargetrps(0, off);
		return false;
	}

	engine_settargetrps(0, off);
	printf("Take 10 ppm at %u ms\nTest engine OK\n", delta(timestamp));
	return true;
}

bool engine_setdirection(enum direction_e direction)
{
	engine_regulvalue = 0;
	pid_enable = false;

	if(direction == off)
	{
		if(!engine_isanydirection())
			return true;

		bool is_cw = engine_isdirectioncw();

		engine_setdirectionoff();

		uint32_t timestamp;
		getsystime(&timestamp);
		while (engine_feedback == ef_full && checkdelay(timestamp, 2000u));
		if (engine_feedback == ef_full)
		{
			if(is_cw)
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

		uint32_t timestamp;
		getsystime(&timestamp);
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


void engine_emergencystop()
{
	pid_enable = false;
	engine_regulvalue = 0;
	delay_ms(20u);

	engine_setdirectionoff();
}

int32_t istate = 0;
int32_t dstate = 0;

bool engine_settargetrps(uint8_t rps, enum direction_e direction)
{
	pid_enable = false;

	//если цель ноль или направление не совпадает, плавный стоп
	if(rps == 0 || direction == off || (direction == cw && engine_isdirectionccw()) || (direction == ccw && engine_isdirectioncw()))
	{
		if(engine_isanydirection())
		{
			tacho_targetrps = 0;
			if (pid_enable)
			{
				uint32_t timestamp;
				getsystime(&timestamp);
				while(tacho_currentrps > 0 && checkdelay(timestamp, 30000u));
			}

			//stop
			pid_enable = false;

/*			if(tacho_currentrps > 0)
			{
				while (engine_regulvalue > eeprom_config.enginestartvalue)
				{
					engine_regulvalue--;
					delay_ms(2u);
				}
			}*/

			engine_regulvalue = 0;
			delay_ms(20u);

			return engine_setdirection(off);
		}
	}

	if(direction != off && rps > 0)
	{
		if(engine_regulvalue == 0)
		{
			//start
			if(!engine_setdirection(direction))
				return false;

			delay_ms(100u);

			//while (engine_regulvalue < eeprom_config.enginestartvalue)
			//{
				//engine_regulvalue++;
				//delay_ms(1u);
			//}

			istate = 0;
			dstate = 0;
		}

		tacho_targetrps = rps;
		pid_enable = true;
	}

	return !ct;
}


inline int16_t pid_process()
{
	int32_t out = eeprom_config.enginestartvalue;
	int32_t delta = (int32_t)tacho_targetrps - (int32_t)tacho_currentrps;

	//p
	int32_t pterm = delta * P;
	out += pterm;

	//i
	istate += delta;
	if(istate > IMAX)
		istate = IMAX;
	if(istate < IMIN)
			istate = IMIN;
	out += istate * I;

	//d
	engine_regulvalue -= (tacho_currentrps - dstate) * D;
	dstate = tacho_currentrps;

	if (out < 0)
		out = 0;
	else if (out > VALUE_FULL)
		out = VALUE_FULL;

	return (int16_t)out;
}

int32_t fmax = -32768;
int32_t fmin = 32768;
inline void engine_crosszero() {
	engine_triakoff();

	TIM4->CR1 &= ~TIM_CR1_CEN; //stop timer
	TIM4->CNT = 0;

	//подсчет значения
	if (pid_enable)
		engine_regulvalue = pid_process();

	if (engine_regulvalue > VALUE_FULL)
		engine_regulvalue = VALUE_FULL;

	if (engine_regulvalue > 0) {
		//запуск таймера задержки
		TIM4->ARR = phaseTable[engine_regulvalue >> 5];
		TIM4->CR1 |= TIM_CR1_CEN;
		TIM4->SR &= ~TIM_SR_UIF;
	}

	if (engine_regulvalue > fmax)
		fmax = engine_regulvalue;
	if (engine_regulvalue < fmin)
		fmin = engine_regulvalue;
}

volatile bool pulse = false;
inline void process_tacho(uint16_t value)
{
	tacho_rawvalue = value;
	//детектор фронта
	if(!pulse && value >= eeprom_config.tachodetectlevel + 10)
	{
		pulse = true;

		if(TIM3->CR1 & TIM_CR1_CEN)
		{
			if(TIM3->CNT != 0)
				tacho_currentrps = 62500 / TIM3->CNT;
		}
		else
			tacho_currentrps = 0;

		TIM3->CR1 &= ~TIM_CR1_CEN;
		TIM3->CNT = 0;
		TIM3->CR1 |= TIM_CR1_CEN; //запуск таймера задержки
	}
	if(pulse && value < eeprom_config.tachodetectlevel)
	{
		pulse = false;
	}
}


inline void process_feedback(enum enginedlfeedbackstate current)
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
