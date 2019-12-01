/*
 * Полоскание
 */

#include <stdbool.h>
#include <stdio.h>
#include "programs.h"
#include "programOptions.h"
#include "door_stages.h"
#include "rinsing_stages.h"
#include "spinning_stages.h"

bool processRinsingProgram(__attribute__((unused)) program programNumber, __attribute__((unused)) programOptions programOptions)
{
	if(programOptions.spinningSpeed > MAX_SPINNING_SPEED)
	{
		printf("Max spinning = %u\n", MAX_SPINNING_SPEED);
		return false;
	}

	if(!stage_door_close())
		return false;

	if(!stage_rinsing(programOptions.rinsingCycles, programOptions.waterLevel))
		return false;

	if(!spinning_go(programOptions.spinningSpeed))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

