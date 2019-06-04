#include <stdbool.h>
#include <stdio.h>
#include "door_driver.h"
#include "pump_driver.h"
#include "crosszero_driver.h"
#include "delay.h"
#include "status.h"

extern volatile bool ct;

bool stage_door_close()
{
	status_set_stage(STATUS_CLOSE_DOOR);
	if(!door_lock())
	  return false;
	return true;
}

bool stage_door_open()
{
	status_set_stage(STATUS_OPEN_DOOR);

	if(!door_unlock())
		return false;

	while (is_crosszero_present() && !ct);
	if(ct)
		return false;

	return true;
}
