#include <stdbool.h>
#include <stdio.h>
#include "programs.h"
#include "programOptions.h"
#include "door_stages.h"
#include "pump_driver.h"
#include "valve_driver.h"
#include "valve_hardware.h"

bool processSinkingProgram(__attribute__((unused)) program programNumber, __attribute__((unused)) programOptions programOptions)
{
	if(!stage_door_close())
		return false;

	if(!sink(15000))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

bool processSinkingProgramWithOpenValve(__attribute__((unused)) program programNumber, __attribute__((unused)) programOptions programOptions)
{
	if(!stage_door_close())
		return false;

	valve_open(both_washmode);

	if(!sink(30000))
		return false;

	valve_close(both_washmode);

	if(!stage_door_open())
		return false;

	return true;
};

