#include <stdbool.h>
#include <stdint-gcc.h>
#include <stdio.h>

#include "door_driver.h"
#include "engine_driver.h"
#include "pump_driver.h"
#include "therm_driver.h"
#include "valve_driver.h"

#include "options.h"
#include "status.h"
#include "eeprom.h"
#include "delay.h"

extern volatile bool ct;

bool testprogram_go(__attribute__((unused)) options args)
{
	status_set_stage(STATUS_SELFTESTING);

	sink_if_water(15000);

	//door
	if(!door_testlock())
		return false;

	delay_ms(5000u);

	//engine
	if (!engine_test())
		return false;
	if(ct)
		return false;

	delay_ms(5000u);

	//valve
	if(!valve_test())
		return false;

	delay_ms(5000u);

	//heater
	if(!therm_test())
		return false;

	//pump
	if (!pump_test())
		return false;
	if(ct)
		return false;

	delay_ms(5000u);

	if(!door_testunlock())
		return false;

	writeconfig();

	printf("All tests OK\n");

	return true;
}
