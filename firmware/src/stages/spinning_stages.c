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

bool spinning_cycle(uint8_t maxrpm);
bool linen_breakdown(uint8_t maxrpm);

extern volatile bool ct;

uint32_t calc_spinning_time(uint8_t maxrpm)
{
	if(!maxrpm)
		return 180;
	else
		return 19.1 * maxrpm + 120;
}

bool spinning_go(uint8_t maxrpm) {
	status_set_stage(STATUS_SPINNING);

	if (!pump_enable())
		return false;

	if(!maxrpm)
	{
		delay_ms(180000u);
	}
	else
	{
		if (!linen_breakdown(maxrpm)) {
			pump_disable();
			return false;
		}

		if (!spinning_cycle(maxrpm)) {
			pump_disable();
			return false;
		}
	}

	return pump_disable();
}

//delay: maxrpm * 16s
bool linen_breakdown(uint8_t maxrpm) {
	for (uint8_t i = 1; i <= maxrpm; i++) {

		//cw
		engine_settargetrps(i, cw);
		delay_ms_with_ct(5000u);
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}

		engine_settargetrps(0, off);
		delay_ms_with_ct(3000u);
		if (ct)
			break;

		//ccw
		engine_settargetrps(i, ccw);
		delay_ms_with_ct(5000u);
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}

		engine_settargetrps(0, off);
		delay_ms_with_ct(3000u);
		if (ct)
			break;
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

