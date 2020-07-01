/*
 * Отжим
 */

#include <engine_driver.h>
#include <pump_driver.h>
#include <stdbool.h>
#include <therm_driver.h>
#include <valve_driver.h>
#include "stm32f10x.h"
#include "delay.h"
#include "status.h"
#include "watchdog.h"

bool spinning_cycle(uint8_t maxrpm);
bool linen_breakdown(uint8_t maxrpm);

extern volatile bool ct;

uint32_t stage_spinning_get_duration(uint8_t maxrpm)
{
	if(!maxrpm)
		return 180;
	else
		return 19.1 * maxrpm + 120;
}

bool stage_spinning(uint8_t maxrpm) {
	status_set_stage(STATUS_SPINNING);

	if (!pump_enable())
		return false;

	if(!maxrpm)
	{
		delay_ms(180000u);
		return true;
	}

	if (!linen_breakdown(maxrpm)) {
		pump_disable();
		return false;
	}

	/*if (!spinning_cycle(maxrpm)) {
		pump_disable();
		return false;
	}*/

	return pump_disable();
}

extern volatile uint16_t engine_current_speed;

//раскладка белья
bool linen_breakdown(uint8_t maxrpm) {
	enum direction_e direction = cw;
	uint32_t timestamp;
	uint16_t minspeed;
	uint16_t maxspeed;

	for (uint8_t i = 1; i <= 10; i++) {

		/*engine_settargetrps(1, direction);
		delay_ms_with_ct(10000u);
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}*/

		engine_settargetrps(2, direction);
		delay_ms_with_ct(15000u);
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}

		engine_settargetrps(3, direction);
		delay_ms_with_ct(5000u);

		minspeed = 9999;
		maxspeed = 0;
		getsystime(&timestamp);
		while (checkdelay(timestamp, 10000) && !ct)
		{
			if(engine_current_speed < minspeed)
				minspeed = engine_current_speed;
			if(engine_current_speed > maxspeed)
				maxspeed = engine_current_speed;

			WDT_RESET;
		}
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}
		printf("Speed 3, min %u, max %u\n", minspeed, maxspeed);

		engine_settargetrps(4, direction);
		delay_ms_with_ct(5000u);

		minspeed = 9999;
		maxspeed = 0;
		getsystime(&timestamp);
		while (checkdelay(timestamp, 10000) && !ct)
		{
			if(engine_current_speed < minspeed)
				minspeed = engine_current_speed;
			if(engine_current_speed > maxspeed)
				maxspeed = engine_current_speed;

			WDT_RESET;
		}
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}
		printf("Speed 4, min %u, max %u\n", minspeed, maxspeed);

		/*engine_settargetrps(5, direction);
		delay_ms_with_ct(5000u);

		minspeed = 9999;
		maxspeed = 0;
		getsystime(&timestamp);
		while (checkdelay(timestamp, 10000) && !ct) {
			if (engine_current_speed < minspeed)
				minspeed = engine_current_speed;
			if (engine_current_speed > maxspeed)
				maxspeed = engine_current_speed;

			WDT_RESET;
		}
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}
		printf("Speed 5, min %u, max %u\n", minspeed, maxspeed);*/

		engine_settargetrps(0, off);
		delay_ms_with_ct(10000u);

		direction = direction == cw ? ccw : cw;
	}

	return !ct;
}

//delay: 3.1 * maxrpm + 120
bool spinning_cycle(uint8_t maxrpm) {

	//start
	uint8_t i = 0;
	do {
		if (!engine_settargetrps(++i, cw))
			return false;
		delay_ms_with_ct(3000u);
	} while (!ct && i < maxrpm);

	if (ct) {
		engine_settargetrps(0, off);
		return false;
	}

	//main
	delay_ms_with_ct(120000u);
	if (ct) {
		engine_settargetrps(0, off);
		return false;
	}

	//stop
	do {
		if (!engine_settargetrps(--i, cw))
			return false;

		delay_ms_with_ct(100u);

	} while (!ct && i > 1);

	if (!engine_settargetrps(0, off))
		return false;

	return !ct;
}

