#include <delay.h>
#include <door_driver.h>
#include <eeprom.h>
#include <error.h>
#include <pump_driver.h>
#include <stdint-gcc.h>
#include <stdio.h>
#include <stm32f10x.h>
#include <systick.h>
#include <valve_driver.h>
#include <valve_hardware.h>

extern volatile bool ct;
extern volatile enum valve_state valve_prewash_state;
extern volatile enum valve_state valve_wash_state;

bool valve_test()
{
	uint32_t timestamp;

	printf("Test prewash valve\n");

	//
	printf("Open prewash valve...\n");
	valve_open(prewash_valve);

	//
	getsystime(&timestamp);
	while (!is_water() && checkdelay(timestamp, 150000u) && !ct);
	if (ct)
	{
		valve_close(prewash_valve);
		return false;
	}
	if (!is_water())
	{
		valve_close(prewash_valve);
		set_error(WATERLEVEL_DOWN_OFF);
		return false;
	}
	eeprom_config.waterleveldowntime = delta(timestamp);
	printf("Valve 1 take lower level on %u ms\n", eeprom_config.waterleveldowntime);

	getsystime(&timestamp);
	//NVIC_DisableIRQ(EXTI1_IRQn);
	while (!is_overflow() && checkdelay(timestamp, eeprom_config.waterleveldowntime * 10) && !ct);
	valve_close(prewash_valve);
	if (!is_overflow())
	{
		set_error(WATERLEVEL_UP_OFF);
		return false;
	}
	if (ct)
	{
		return false;
	}

	eeprom_config.waterleveluptime = delta(timestamp);

	printf("Valve 1 take higher level on %u ms\nTest valve 1 OK\nTest wash valve...\nSink...\n", eeprom_config.waterleveluptime);

	if (!sink(30000U))
		return false;
	//NVIC_EnableIRQ(EXTI1_IRQn);
	//
	printf("Open wash valve...\n");
	valve_open(conditioner_valve);

	//
	getsystime(&timestamp);
	while (!is_water() && checkdelay(timestamp, 150000u) && !ct);
	valve_close(conditioner_valve);
	if (!is_water())
	{
		set_error(WATERLEVEL_DOWN_OFF);
		return false;
	}
	if (ct)
	{
		return false;
	}

	printf("Valve 2 take lower level on %u\nTest valve 2 OK\n", delta(timestamp));

	return true;
}

bool valve_drawwater(enum valve_e valve, uint8_t level) {
	//TODO: engine off door interrupt
	/*	if(!is_door_closed())
	 {
	 set_error(TRY_OPEN_WATER_WITH_OPEN_DOOR);
	 return false;
	 }*/

	if (is_water()) {
		if (!sink(15000))
			return false;
	}

	valve_open(valve);

	uint32_t timestamp;
	getsystime(&timestamp);
	while (!is_water() && checkdelay(timestamp, 120000u) && !ct)
		;
	if (!checkdelay(timestamp, 120000u)) {
		set_error(WATERLEVEL_DOWN_OFF);
		valve_close(valve);
		return false;
	}
	if (ct) {
		valve_close(valve);
		return false;
	}

	delay_ms_with_ct(eeprom_config.waterleveluptime * level / 100);
	valve_close(valve);
	return true;
}

void valve_open(enum valve_e valve)
{
	switch (valve)
	{
	case prewash_valve:
		valve_prewash_state = valve_opened;
		delay_ms(100);
		valve_prewash_state = valve_retention;
		break;
	case conditioner_valve:
		valve_wash_state = valve_opened;
		delay_ms(100);
		valve_wash_state = valve_retention;
		break;
	case both_washmode:
		valve_prewash_state = valve_opened;
		valve_wash_state = valve_opened;
		delay_ms(100);
		valve_prewash_state = valve_retention;
		valve_wash_state = valve_retention;
		break;
	}
}

void valve_close(enum valve_e valve)
{
	switch (valve)
	{
	case prewash_valve:
		valve_prewash_state = valve_closed;
		break;
	case conditioner_valve:
		valve_wash_state = valve_closed;
		break;
	case both_washmode:
		valve_prewash_state = valve_closed;
		valve_wash_state = valve_closed;
		break;
	}
}
