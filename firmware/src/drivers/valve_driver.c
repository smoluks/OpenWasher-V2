#include <stdio.h>
#include <stdbool.h>
#include "valve_driver.h"
#include "pump_driver.h"
#include "valve_hardware.h"
#include "systick.h"
#include "eeprom.h"
#include "delay.h"

extern volatile bool ct;
extern volatile enum valve_state valve_prewash_state;
extern volatile enum valve_state valve_wash_state;

bool valve_test()
{
	if (is_water()) {
		printf("Valve test need no water\n");
		return false;
	}

	//prewash
	printf("Test prewash valve\nOpen prewash valve...\n");
	uint32_t timestamp = get_systime();
	valve_open(prewash_valve);

	while (!is_water() && checkdelay(timestamp, 150000u) && !ct);
	if (ct)
	{
		valve_close(prewash_valve);
		return false;
	}
	if (!checkdelay(timestamp, 150000u))
	{
		valve_close(prewash_valve);
		set_error(VALVE1_NOT_OPEN);
		return false;
	}
	eeprom_config.waterleveldowntime = delta(timestamp);
	printf("Prewash valve make lower level on %lu ms\n", eeprom_config.waterleveldowntime);

	timestamp = get_systime();
	//NVIC_DisableIRQ(EXTI1_IRQn);
	while (!is_overflow() && checkdelay(timestamp, eeprom_config.waterleveldowntime * 10) && !ct);
	valve_close(prewash_valve);
	if (!checkdelay(timestamp, eeprom_config.waterleveldowntime * 10))
	{
		set_error(WATERLEVEL_UP_OFF);
		return false;
	}
	if (ct)
		return false;

	eeprom_config.waterleveluptime = delta(timestamp);
	printf("Prewash valve make higher level on %lu ms\nTest prewash valve OK\nTest conditioner valve...\nSink...\n", eeprom_config.waterleveluptime);

	if (!sink(30000u))
		return false;

	//NVIC_EnableIRQ(EXTI1_IRQn);
	//
	printf("Open conditioner valve...\n");
	valve_open(conditioner_valve);

	timestamp = get_systime();
	while (!is_water() && checkdelay(timestamp, 150000u) && !ct);
	valve_close(conditioner_valve);
	if (!checkdelay(timestamp, 150000u))
	{
		set_error(VALVE2_NOT_OPEN);
		return false;
	}
	if (ct)
		return false;

	printf("Conditioner valve make take lower level on %lu\nTest conditioner valve OK\n", delta(timestamp));

	return true;
}

bool valve_drawwater(enum valve_e valve, uint8_t level) {
	//TODO: engine off door interrupt
	/*	if(!is_door_closed())
	 {
	 set_error(TRY_OPEN_WATER_WITH_OPEN_DOOR);
	 return false;
	 }*/

	//sink
	if (!sink_if_water(15000))
		return false;

	valve_open(valve);

	uint32_t timestamp = get_systime();
	while (!is_water() && checkdelay(timestamp, eeprom_config.waterleveldowntime + 15000) && !ct);
	if (!is_water()) {
		set_error(NO_WATER);
		valve_close(valve);
		return false;
	}
	if (ct) {
		valve_close(valve);
		return false;
	}

	uint32_t watersettime = eeprom_config.waterleveluptime * level / 100;
	while (!is_overflow() && checkdelay(timestamp, watersettime) && !ct);
	if (checkdelay(timestamp, watersettime)) {
		set_error(OVERFLOW);
		return false;
	}

	valve_close(valve);
	return !ct;
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
