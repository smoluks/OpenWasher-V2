/*
 * pidprogram.c
 *
 *  Created on: 2 èþë. 2019 ã.
 *      Author: Shironeko
 */

#include <stdbool.h>
#include "system_stm32f10x.h"
#include "pidprogram.h"
#include "pump_driver.h"
#include "door_stages.h"
#include "engine_driver.h"
#include "programs.h"
#include "programOptions.h"

bool processPidProgram(__attribute__((unused)) program programNumber, __attribute__((unused)) programOptions programOptions) {
	sink_if_water(15000);

	if (!stage_door_close())
		return false;

	if (!engine_calibratepid())
		return false;

	if (!stage_door_open())
		return false;

	return true;
}



