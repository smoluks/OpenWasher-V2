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

bool washing_cycle(uint32_t endtime, uint8_t rps, enum valve_e valve, uint8_t waterlevel);

uint32_t stage_wash_get_duration(uint8_t duration){
	return duration * 60 * 1000;
}

//
//wash cycle
//temp - temperature in celsius
//duration - duration in minutes
//
bool stage_wash(uint8_t temp, uint8_t duration, uint8_t rps, enum valve_e valve, uint8_t waterlevel)
{
	int32_t fulltime = duration * 60 * 1000 - 15000 - (temp - 20) * 4000;

	if(!valve_drawwater(valve, waterlevel))
		return false;

	if(!set_temperature(temp))
		return false;

	bool error = false;

	while(fulltime > 0)
	{
		uint32_t cycletime = fulltime > 90000 ? 60000 : fulltime;

		if(!washing_cycle(cycletime, rps, valve, waterlevel))
		{
			error = true;
			break;
		}

		fulltime -= cycletime;
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
bool washing_cycle(uint32_t cycletime, uint8_t rps, enum valve_e valve, uint8_t waterlevel)
{
	//set speed
	if (!engine_settargetrps(rps, direction))
		return false;

	//check water
	if (!is_water()) {
		if (!valve_drawwater(valve, waterlevel))
		{
			engine_settargetrps(0, off);
			return false;
		}
	}

	delay_ms_with_ct(cycletime > 10000u ? cycletime - 10000u : cycletime);
	//interrupt
	if (ct){
		engine_settargetrps(0, off);
		return false;
	}

	//stop
	if (!engine_settargetrps(0, off) || ct)
		return false;

	if(cycletime == 0)
		return true;

	delay_ms_with_ct(10000u);
	direction = direction == cw ? ccw : cw;

	return true;
}

