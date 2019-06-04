#include <stdbool.h>
#include <stdio.h>
#include "systick.h"
#include "door_driver.h"
#include "engine_driver.h"
#include "crosszero_driver.h"
#include "door_hardware.h"
#include "valve_hardware.h"

extern volatile bool ct;

bool door_testlock()
{
	printf("Door lock test\nCheck feedback...\n");

	//---check locker feedback---
	unlock_door();

	uint32_t timestamp = get_systime();
	while (get_engine_feedback() != ef_full && checkdelay(timestamp, 1000));
	if(get_engine_feedback() != ef_full)
	{
		set_error(NO_LOCKER);
		return false;
	}
	printf("Full feedback present at %lu ms\n", delta(timestamp));

	//---check locker feedback on locker triak on---
	lock_door();

	getsystime(&timestamp);
	while (get_engine_feedback() != ef_half && checkdelay(timestamp, 1000));
	if (get_engine_feedback() != ef_half)
	{
		set_error(BAD_DOOR_TRIAK);
		return false;
	}
	printf("Lock feedback present at %lu ms\nWait to close...\n", delta(timestamp));

	//---wait door close---
	getsystime(&timestamp);
	while(!is_crosszero_present() && !ct);
	if(ct)
		return false;

	printf("Door lock at %lu ms\nDoor lock test OK\n", delta(timestamp));

	return true;
}

bool door_testunlock()
{
	printf("Door unlock test\nCheck feedback...\n");
	if(!is_crosszero_present())
	{
		printf("Already opened\n");
		return false;
	}

	//---check locker feedback---
	unlock_door();

	uint32_t timestamp = get_systime();
	while (get_engine_feedback() != ef_full && checkdelay(timestamp, 3000U));
	if (get_engine_feedback() != ef_full)
	{
		set_error(BAD_DOOR_TRIAK2);
		return false;
	}
	printf("Feedback present at %lu\nWait to open...\n", delta(timestamp));

	//---wait door open---
	while(is_crosszero_present() && !ct);
	if(ct)
		return false;

	printf("Door unlock at %lu\nDoor unlock test OK\n", delta(timestamp));

	return true;
}

bool door_lock()
{
	lock_door();

	//---check locker feedback---
	uint32_t timestamp = get_systime();
	while (get_engine_feedback() != ef_half && checkdelay(timestamp, 1000));
	if (get_engine_feedback() != ef_half)
	{
		set_error(BAD_DOOR_TRIAK);
		return false;
	}

	//---wait door close---
	while (!is_crosszero_present() && !ct);
	if(ct)
		return false;

	return true;
}

bool door_unlock() {
	if (is_water()) {
		set_error(TRY_OPEN_DOOR_WITH_WATER);
		return false;
	}

	unlock_door();

	//---check locker feedback---
	uint32_t timestamp = get_systime();
	while (get_engine_feedback() != ef_full && checkdelay(timestamp, 1000));
	if (get_engine_feedback() != ef_full) {
		set_error(BAD_DOOR_TRIAK2);
		return false;
	}

	return true;
}
