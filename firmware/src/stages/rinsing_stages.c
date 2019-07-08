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

bool stage_rinsing(uint8_t count, uint8_t waterlevel)
{
	if(!count)
		return true;

	status_set_stage(STATUS_RINSING);

	for(int i = 0; i<count; i++)
	{
		if(!engine_settargetrps(1, ccw))
			return false;

		if(!valve_drawwater(conditioner_valve, waterlevel + 10))
		{
			engine_settargetrps(0, off);
			return false;
		}

		uint32_t endtime = get_systime() + (i * 5 + 1 )* 60000u;
		while(!check_time_passed(endtime))
		{
			if (!is_water()) {
					if (!valve_drawwater(conditioner_valve, waterlevel + 10))
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


