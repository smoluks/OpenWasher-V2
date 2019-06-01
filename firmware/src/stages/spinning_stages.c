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

bool spinning_cycle(uint8_t maxrpm, enum direction_e direction);

extern volatile bool ct;

bool linen_breakdown() {
	enum direction_e direction = cw;

	for (uint8_t i = 0; i < 6; i++) {
		engine_settargetrps(5, direction);
		delay_ms_with_ct(5000);
		if (ct) {
			engine_settargetrps(0, off);
			break;
		}

		engine_settargetrps(0, off);
		delay_ms_with_ct(3000u);
		if (ct)
			break;

		direction = direction == cw ? ccw : cw;
	}
	return !ct;
}

bool spinning_go(uint8_t maxrpm) {
	status_set_stage(STATUS_SPINNING);

	if (!pump_enable())
		return false;

	if (!linen_breakdown()) {
		pump_disable();
		return false;
	}

	if (!spinning_cycle(maxrpm >> 1, cw) || !spinning_cycle(maxrpm, ccw)) {
		pump_disable();
		return false;
	}

	return pump_disable();
}

bool spinning_cycle(uint8_t maxrpm, enum direction_e direction) {
	uint8_t i = 0;
	do {
		if (!engine_settargetrps(++i, direction))
			return false;

		delay_ms_with_ct(2000u);
		if (ct)
			break;
	} while (i <= maxrpm);
	if (ct) {
		engine_settargetrps(0, off);
		return false;
	}

	//
	delay_ms_with_ct(30000u);
	if (ct) {
		engine_settargetrps(0, off);
		return false;
	}

	//
	do {
		if (!engine_settargetrps(--i, direction))
			return false;

		delay_ms_with_ct(100u);
		if (ct)
			break;
	} while (i > 0);

	if (!engine_settargetrps(0, off))
		return false;

	return !ct;
}

