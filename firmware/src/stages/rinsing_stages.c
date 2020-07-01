#include <engine_driver.h>
#include <pump_driver.h>
#include <stdbool.h>
#include <therm_driver.h>
#include <valve_driver.h>
#include "valve_hardware.h"
#include "stm32f10x.h"
#include "delay.h"
#include "status.h"
#include "systick.h"

extern volatile bool ct;

uint32_t stage_rinsing_get_duration(uint8_t count)
{
	uint32_t duration = 0;

	for(int i = 0; i<count; i++)
	{
		duration += (i * 5 + 1 )* 60000u;
	}

	return duration;
}


bool stage_rinsing(uint8_t count, uint8_t speed, uint8_t waterlevel)
{
	if(!count)
		return true;

	status_set_stage(STATUS_RINSING);

	for(int i = 0; i<count; i++)
	{
		if(!engine_settargetrps(speed, ccw))
			return false;

		if(!valve_drawwater(conditioner_valve, waterlevel))
		{
			engine_settargetrps(0, off);
			return false;
		}

		uint32_t endtime = get_systime() + (i * 5 + 1 )* 60000u;
		while(!check_time_passed(endtime))
		{
			if (!is_water()) {
					if (!valve_drawwater(conditioner_valve, waterlevel))
						return false;
				}
		}

		if(!engine_settargetrps(0, off))
			return false;

		if(!sink(15000) || ct)
			return false;
	}

	return !ct;
}


