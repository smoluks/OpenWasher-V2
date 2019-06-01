/*
 * Полоскание
 */

#include <door_stages.h>
#include <rinsing_stages.h>
#include <stdbool.h>
#include <stdint-gcc.h>

bool rinsingprogram_go(uint8_t number, uint8_t* args)
{
	printf("Close door\n");
	if(!stage_door_close())
		return false;

	printf("Rinsing\n");
	if(!stage_rinsing(3))
		return false;

	printf("Spinning\n");
	if(!spinning_go(20))
		return false;

	printf("Open door\n");
	if(!stage_door_open())
		return false;

	return true;
}

bool delicaterinsingprogram_go(uint8_t number, uint8_t* args)
{
	if(!stage_door_close())
		return false;

	if(!stage_rinsing(3))
		return false;

	if(!spinning_go(2))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}



