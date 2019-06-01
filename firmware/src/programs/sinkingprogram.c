/*
 * Слив
 */

#include <door_stages.h>
#include <pump_driver.h>
#include <stdbool.h>
#include <stdint-gcc.h>
#include <valve_driver.h>
#include <valve_hardware.h>

bool sink_go(uint8_t number, uint8_t* args)
{
	if(!stage_door_close())
		return false;

	if(!sink(15000))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

bool sink_open_valve_go(uint8_t number, uint8_t* args)
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

