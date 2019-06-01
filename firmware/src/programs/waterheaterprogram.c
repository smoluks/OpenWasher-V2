/*
 * Нагрев воды и слив
 */

#include <stdbool.h>
#include <stdint-gcc.h>
#include <stdio.h>
#include <delay.h>
#include <door_stages.h>
#include <pump_driver.h>
#include <therm_driver.h>
#include <valve_driver.h>

extern volatile bool ct;

bool waterheater_go(uint8_t number, uint8_t* args)
{
	if(!stage_door_close())
			return false;

	while(!ct)
	{
		if(!valve_drawwater(both_washmode, 100u))
			break;

		if(!set_temperature(70))
			break;

		while(get_temperature() < 70)
		{
			printf("T %u\n", get_temperature());

			delay_ms(10000u);
		}

		if(!set_temperature(0))
			break;

		if(!sink(15000))
			break;

		delay_ms(30000u);
	}

	if(!set_temperature(0))
		return false;

	if(!sink(15000))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}


