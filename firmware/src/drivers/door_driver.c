#include <error.h>
#include <stdio.h>
#include <stdint.h>
#include "systick.h"
#include "valve_hardware.h"
#include "door_driver.h"
#include "door_hardware.h"
#include "engine_driver.h"
#include "crosszero_driver.h"

extern volatile enum enginedlfeedbackstate engine_feedback;
extern volatile bool ct;

volatile uint8_t goodperiods = 0;
volatile bool flag = false;

bool door_testlock()
{
	uint32_t timestamp;

	printf("Door lock test\nCheck feedback...\n");

	unlock_door();

	getsystime(&timestamp);
	while (engine_feedback != ef_full && checkdelay(timestamp, 1000));
	if(engine_feedback != ef_full)
	{
		set_error(NO_LOCKER);
		return false;
	}
	printf("Full feedback present at %u ms\n", delta(timestamp));

	lock_door();

	getsystime(&timestamp);
	while (engine_feedback != ef_half && checkdelay(timestamp, 1000));
	if (engine_feedback != ef_half)
	{
		set_error(BAD_DOOR_TRIAK);
		return false;
	}
	printf("Lock feedback present at %u ms\nWait to close...\n", delta(timestamp));

	getsystime(&timestamp);
	while(!is_crosszero_present() && !ct);
	if(ct)
		return false;

	printf("Door lock at %u ms\nDoor lock test OK\n", delta(timestamp));

	return true;
}

bool door_testunlock()
{
	uint32_t timestamp;

	printf("Door unlock test\nCheck feedback...\n");

	if(!is_crosszero_present())
	{
		printf("Already opened\n");
		return false;
	}

	unlock_door();

	getsystime(&timestamp);
	while (engine_feedback != ef_full && checkdelay(timestamp, 3000U));
	if (engine_feedback != ef_full)
	{
		printf("Bad door feedback: %u\n", engine_feedback);
		set_error(BAD_DOOR_TRIAK2);
		return false;
	}
	printf("Feedback present at %u\nWait to open...\n", delta(timestamp));

	while(is_crosszero_present() && !ct);
	if(ct)
		return false;

	printf("Door unlock at %u\nDoor unlock test OK\n", delta(timestamp));
	return true;
}

inline bool door_lock()
{
	uint32_t timestamp;

	lock_door();

	getsystime(&timestamp);
	while (engine_feedback != ef_half && checkdelay(timestamp, 1000));
	if (engine_feedback != ef_half)
	{
		set_error(BAD_DOOR_TRIAK);
		return false;
	}
	return true;
}

inline bool door_unlock()
{
	uint32_t timestamp;

	if (is_water())
	{
		set_error(TRY_OPEN_DOOR_WITH_WATER);
		return false;
	}
	else
	{
		unlock_door();

		getsystime(&timestamp);
		while (engine_feedback != ef_full && checkdelay(timestamp, 1000));
		if (engine_feedback != ef_full)
		{
			set_error(BAD_DOOR_TRIAK2);
			return false;
		}

		return true;
	}
}
