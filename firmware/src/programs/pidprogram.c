/*
 * pidprogram.c
 *
 *  Created on: 2 èþë. 2019 ã.
 *      Author: Shironeko
 */

#include "system_stm32f10x.h"
#include <stdbool.h>
#include "options.h"

bool pid_go(__attribute__((unused))  options args) {
	sink_if_water(15000);

	if (!stage_door_close())
		return false;

	if (!engine_calibratepid())
		return false;

	if (!stage_door_open())
		return false;

	return true;
}



