#include <delay.h>
#include <door_driver.h>
#include <door_stages.h>
#include <eeprom.h>
#include <engine_driver.h>
#include <engine_driver.h>
#include <stdbool.h>
#include <stdint-gcc.h>
#include <stdio.h>
#include <hardware.h>
#include <pump_driver.h>
#include <therm_driver.h>
#include <valve_driver.h>

extern volatile bool ct;

bool testprogram_go(uint8_t number, uint8_t* args)
{
	//door
	if(!door_testlock())
		return false;

	delay_ms(5000u);

	//pump
	if (!pump_test())
		return false;
	if(ct)
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

	delay_ms(15000u);

	if(!door_testunlock())
		return false;

	writeconfig();

	printf("All tests OK\n");

	return true;
}
