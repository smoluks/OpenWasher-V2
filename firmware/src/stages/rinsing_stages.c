#include <engine_driver.h>
#include <pump_driver.h>
#include <stdbool.h>
#include <therm_driver.h>
#include <valve_driver.h>
#include "stm32f10x.h"
#include "delay.h"
#include "status.h"

extern volatile bool ct;

bool stage_rinsing(uint8_t count)
{
	if(!count)
		return true;

	status_set_stage(STATUS_RINSING);

	while(count--)
	{
		engine_settargetrps(2, ccw);

		if(!valve_drawwater(conditioner_valve, 10))
		{
			engine_settargetrps(0, off);
			return false;
		}

		delay_ms_with_ct(5000u);

		engine_settargetrps(0, off);
		if(ct)
			return false;

		enum direction_e direction = cw;
		for(int i = 0; i < 5; i++)
		{
			engine_settargetrps(2, direction);
			direction = direction == cw ? ccw : cw;

			delay_ms_with_ct(55000u);
			engine_settargetrps(0, off);
			if(ct)
				break;

			delay_ms_with_ct(5000u);
			if(ct)
				break;
		}

		if(!sink(15000))
			return false;

		if(ct)
			return false;
	}

	return !ct;
}


