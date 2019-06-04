/*
 * Полоскание
 */

#include <stdbool.h>
#include <stdio.h>
#include "options.h"
#include "door_stages.h"
#include "rinsing_stages.h"
#include "spinning_stages.h"

bool rinsingprogram_go(__attribute__((unused)) options args)
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

bool delicaterinsingprogram_go(__attribute__((unused)) options args)
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



