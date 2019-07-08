/*
 * Полоскание
 */

#include <stdbool.h>
#include <stdio.h>
#include "options.h"
#include "door_stages.h"
#include "rinsing_stages.h"
#include "spinning_stages.h"

bool rinsingprogram_go(options args)
{
	//rinsing cycles
	uint8_t rinsingcycles = args.rinsingcycles != 0xFF ? args.rinsingcycles : 3;

	//spinning speed
	uint8_t spinningspeed = args.spinningspeed != 0xFF ? args.spinningspeed : args.number == 12 ? 10 : 0;

	//waterlevel
	uint8_t waterlevel = args.waterlevel != 0xFF ? args.waterlevel : 10;
	if(waterlevel > 100)
		waterlevel = 100;

	if(spinningspeed > 20)
	{
		printf("Max spinning = 20\n");
			return false;
	}

	if(!stage_door_close())
		return false;

	if(!stage_rinsing(rinsingcycles, waterlevel))
		return false;

	if(!spinning_go(spinningspeed))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

