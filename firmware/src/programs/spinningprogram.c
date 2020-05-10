/*
 * Отжим
 */
#include <stdbool.h>
#include <stdio.h>
#include "door_stages.h"
#include "pump_driver.h"
#include "rinsing_stages.h"
#include "spinning_stages.h"
#include "programs.h"
#include "programOptions.h"

bool processSpinningProgram(__attribute__((unused)) program programNumber, programOptions programOptions)
{
	if(programOptions.spinningSpeed > MAX_SPINNING_SPEED)
	{
		printf("Max spinning = %u\n", MAX_SPINNING_SPEED);
		return false;
	}

	if(!stage_door_close())
		return false;

	if(!stage_spinning(programOptions.spinningSpeed))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}
