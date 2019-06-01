#include <stdbool.h>
#include <delay.h>
#include <door_stages.h>
#include <pump_driver.h>
#include <rinsing_stages.h>
#include <stdint-gcc.h>
#include <stdio.h>
#include <valve_driver.h>
#include <wash_stages.h>
#include <status.h>

const uint8_t washtemperature[11] = {
		80, 80, 60, 40,
		30, 60, 50, 40,
		30, 40, 30
};
const uint8_t washdelay[11] = {
		137, 120, 105, 72,
		60, 77, 73, 58,
		30, 50, 45
};
const uint8_t spinningrps[11] = {
		20, 20, 20, 20,
		20, 5, 5, 5,
		5, 5, 0
};

bool wash_go(uint8_t number, uint8_t* args)
{
	if(number > 11)
		return false;

	uint8_t temperature = args[0] != 0xFF ? args[0] : washtemperature[number - 1];
	if(temperature > 80)
	{
		printf("Tmax = 80\n");
		return false;
	}

	uint8_t delay = args[1] != 0xFF ? args[1] : washdelay[number - 1];

	uint8_t spinningspeed = args[2] != 0xFF ? args[2] : spinningrps[number - 1];
	if(spinningspeed > 20)
	{
		printf("Max spinning = 20\n");
		return false;
	}

	uint8_t waterlevel = args[3] != 0xFF ? args[3] : 10;
	if(waterlevel > 100)
		waterlevel = 100;

	uint8_t rinsingcycles = args[4] != 0xFF ? args[4] : 3;

	bool prewash = number == 1;

	printf("Start washing at t: %u, time: %u, spinning speed: %u, water level: %u, rinsing cycles: %u, prewash: %s\n", temperature, delay, spinningspeed, waterlevel, rinsingcycles, prewash ? "true" : "false");

	if(!stage_door_close())
		return false;

	if (is_water() && !sink(15000))
		return false;

	if(prewash)
	{
		status_set_stage(STATUS_PREWASHING);
		if(!stage_wash(temperature, 20, 3, prewash_valve, waterlevel))
			return false;
	}

	status_set_stage(STATUS_WASHING);
	if(!stage_wash(temperature, delay, 3, both_washmode, waterlevel))
		return false;

	if(rinsingcycles)
	{
		if(!stage_rinsing(rinsingcycles))
			return false;
	}

	if(spinningspeed)
	{
		if(!spinning_go(spinningspeed))
			return false;
	}

	if(!stage_door_open())
		return false;

	return true;
}

