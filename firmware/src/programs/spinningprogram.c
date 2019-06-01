/*
 * Отжим
 */

#include <door_stages.h>
#include <pump_driver.h>
#include <rinsing_stages.h>
#include <stdbool.h>

bool spinningprogram_go(uint8_t number, uint8_t* args)
{
	if(!stage_door_close())
		return false;

	if(!spinning_go(20))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

bool delicatespinningprogram_go(uint8_t number, uint8_t* args)
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
