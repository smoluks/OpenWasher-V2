#include <stdbool.h>
#include <stdio.h>
#include <stdint.h>
#include "programs.h"
#include "programOptions.h"
#include "engine_driver.h"
#include "delay.h"
#include "door_stages.h"
#include "status.h"
#include "grindingbrushesprogram.h"

extern volatile bool ct;

bool processGrindingBrushes(__attribute__((unused)) program programNumber, programOptions programOptions)
{
	if(programOptions.spinningSpeed > MAX_SPINNING_SPEED)
	{
		printf("Max spinning = %u\n", MAX_SPINNING_SPEED);
		return false;
	}

	if(!stage_door_close())
		return false;

	status_set_stage(STATUS_GRINDINGBRUSHES);

	engine_settargetrps(programOptions.spinningSpeed, cw);
	delay_ms_with_ct(programOptions.delay * 30000);
	engine_settargetrps(0, off);
	if (ct)
		return false;

	engine_settargetrps(programOptions.spinningSpeed, ccw);
	delay_ms_with_ct(programOptions.delay * 30000);
	engine_settargetrps(0, off);
	if (ct)
		return false;

	if(!stage_door_open())
		return false;

	return true;
}
