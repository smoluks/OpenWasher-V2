#include <stdbool.h>
#include <stdio.h>
#include "washingprogram.h"
#include "door_stages.h"
#include "rinsing_stages.h"
#include "wash_stages.h"
#include "spinning_stages.h"
#include "pump_driver.h"
#include "valve_driver.h"
#include "status.h"
#include "delay.h"
#include "options.h"
#include "washprograms.h"

bool wash_go(options args)
{
	if(args.number == 0 || args.number > 11)
	{
		printf("Wash number 1 - 11\n");
		return false;
	}
	washprogram program = washprograms[args.number - 1];

	//t
	uint8_t temperature = args.temperature != 0xFF ? args.temperature : program.temperature;
	if(temperature > 80)
	{
		printf("Tmax = 80\n");
		return false;
	}

	//delay
	uint8_t delay = args.delay != 0xFF ? args.delay : program.delay;

	//washing speed
	uint8_t washingspeed = args.washingspeed != 0xFF ? args.washingspeed : program.washingspeed;

	//spinning speed
	uint8_t spinningspeed = args.spinningspeed != 0xFF ? args.spinningspeed : program.spinningspeed;
	if(spinningspeed > 20)
	{
		printf("Max spinning = 20\n");
		return false;
	}

	//waterlevel
	uint8_t waterlevel = args.waterlevel != 0xFF ? args.waterlevel : 3;
	if(waterlevel > 100)
		waterlevel = 100;

	//rinsing cycles
	uint8_t rinsingcycles = args.rinsingcycles != 0xFF ? args.rinsingcycles : 3;

	//prewash
	bool prewash = args.number == 1;

	printf("Start washing at t: %u, time: %u, spinning speed: %u, water level: %u, rinsing cycles: %u, prewash: %s\n", temperature, delay, spinningspeed, waterlevel, rinsingcycles, prewash ? "true" : "false");

	if(!stage_door_close())
		return false;

	if(prewash)
	{
		status_set_stage(STATUS_PREWASHING);
		if(!stage_wash(temperature, 20, washingspeed, prewash_valve, waterlevel))
			return false;
	}

	status_set_stage(STATUS_WASHING);
	if(!stage_wash(temperature, delay, washingspeed, both_washmode, waterlevel))
		return false;

	if(!stage_rinsing(rinsingcycles, waterlevel))
		return false;

	if(!spinning_go(spinningspeed))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

