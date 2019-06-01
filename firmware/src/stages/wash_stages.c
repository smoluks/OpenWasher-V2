/*
 * Стирка
 * Created on: 23 сент. 2018 г.
 * Author: Администратор
 */

#include <engine_driver.h>
#include <pump_driver.h>
#include <stdbool.h>
#include <stdio.h>
#include <therm_driver.h>
#include <valve_hardware.h>
#include <valve_driver.h>
#include "stm32f10x.h"
#include "delay.h"

extern volatile bool ct;
extern volatile uint32_t systime;

uint32_t endtime;

bool washing_cycle(uint8_t rps, enum valve_e valve, uint8_t waterlevel);

//
//wash cycle
//temp - temperature in celsius
//duration - duration in minutes
//
bool stage_wash(uint8_t temp, uint8_t duration, uint8_t rps, enum valve_e valve, uint8_t waterlevel)
{
	endtime = systime + duration*60*1000;

	if(!valve_drawwater(valve, waterlevel))
		return false;

	if(!set_temperature(temp))
		return false;

	bool error = false;

	while(!ct && endtime - systime > 5000u && endtime - systime < 2147483647u)
	{
		if(!washing_cycle(rps, valve, waterlevel))
		{
			error = true;
			break;
		}
	}

	if(!set_temperature(0))
		error = true;

	if(!engine_settargetrps(0, off))
		error = true;

	if(!sink(15000))
		error = true;

	//остывание бака и двигателя
	delay_ms_with_ct((get_temperature() - 20) * 4000u);

	return !error && !ct;
}

enum direction_e direction = cw;
bool washing_cycle(uint8_t rps, enum valve_e valve, uint8_t waterlevel)
{
	if (!is_water()) {
		if (!valve_drawwater(valve, waterlevel + 10))
			return false;
	}

	if (!engine_settargetrps(rps, direction))
		return false;

	delay_ms_with_ct(endtime - systime > 55000u ? 55000u : endtime - systime);
	if (ct || endtime < systime)
		return false;

	if (!engine_settargetrps(0, off))
		return false;
	if (ct)
		return false;

	delay_ms_with_ct(endtime - systime > 5000u ? 5000u : endtime - systime);
	direction = direction == cw ? ccw : cw;

	return true;
}

