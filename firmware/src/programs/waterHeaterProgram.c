/*
 * Нагрев воды и слив
 */

#include <stdbool.h>
#include <stdint.h>
#include <stdio.h>
#include "delay.h"
#include "door_stages.h"
#include "pump_driver.h"
#include "therm_driver.h"
#include "valve_driver.h"
#include "programs.h"
#include "programOptions.h"

extern volatile bool ct;

bool processWaterHeaterCommand(__attribute__((unused))  program programNumber,
		programOptions programOptions) {
	if (programOptions.temperature == 0xFF) {
		printf("Please set temperature");
		return false;
	}
	if (programOptions.waterLevel == 0xFF) {
		printf("Please set waterLevel");
		return false;
	}

	if (programOptions.temperature > MAX_TEMPERATURE) {
		printf("Tmax = %u\n", MAX_TEMPERATURE);
		return false;
	}
	if (programOptions.waterLevel > 100) {
		programOptions.waterLevel = 100;
	}

	printf("Start water heater at t: %u, water level: %u\n",
			programOptions.temperature, programOptions.waterLevel);

	if (!stage_door_close())
		return false;

	while (!ct) {
		if (!valve_drawwater(conditioner_valve, programOptions.waterLevel))
			break;

		if (!set_temperature(programOptions.temperature))
			break;

		while (get_temperature() < programOptions.temperature) {
			printf("T %u\n", get_temperature());
			delay_ms(10000u);
		}

		if (!set_temperature(0))
			break;

		if (!sink(15000))
			break;

		delay_ms(30000u);
	}

	if (!set_temperature(0))
		return false;

	if (!sink(15000))
		return false;

	if (!stage_door_open())
		return false;

	return true;
}


