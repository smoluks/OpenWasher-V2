/*
 * Отжим
 */
#include <stdbool.h>
#include "door_stages.h"
#include "pump_driver.h"
#include "rinsing_stages.h"
#include "spinning_stages.h"
#include "options.h"

bool spinningprogram_go(__attribute__((unused)) options args)
{
	if(!stage_door_close())
		return false;

	if(!spinning_go(20))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

bool delicatespinningprogram_go(__attribute__((unused)) options args)
{
	if(!stage_door_close())
		return false;

	if (!sink(15000))
		return false;

	if(!spinning_go(5))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}
