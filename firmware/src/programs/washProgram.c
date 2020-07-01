#include <stdbool.h>
#include <stdio.h>
#include "washProgram.h"
#include "door_stages.h"
#include "rinsing_stages.h"
#include "wash_stages.h"
#include "spinning_stages.h"
#include "pump_driver.h"
#include "valve_driver.h"
#include "status.h"
#include "delay.h"
#include "programs.h"
#include "programOptions.h"

bool processWashProgram(program programNumber, programOptions programOptions)
{
	//---validation---
	if(programNumber < 1 || programNumber > 11)
	{
		printf("Wash number 1 - 11\n");
		return false;
	}

	if (programOptions.delay == 0xFF) {
		printf("Please set delay");
		return false;
	}
	if (programOptions.rinsingCycles == 0xFF) {
		printf("Please set rinsingCycles");
		return false;
	}

	if (programOptions.temperature == 0xFF) {
		printf("Please set temperature");
		return false;
	}
	if(programOptions.temperature > MAX_TEMPERATURE)
	{
		printf("Tmax = %u\n", MAX_TEMPERATURE);
		return false;
	}

	if (programOptions.washingSpeed == 0xFF) {
		printf("Please set washingSpeed");
		return false;
	}
	if(programOptions.washingSpeed > MAX_WASHING_SPEED)
	{
		printf("Max washing = %u\n", MAX_WASHING_SPEED);
		return false;
	}

	if (programOptions.spinningSpeed == 0xFF) {
		printf("Please set spinningSpeed");
		return false;
	}
	if(programOptions.spinningSpeed > MAX_SPINNING_SPEED)
	{
		printf("Max spinning = %u\n", MAX_SPINNING_SPEED);
		return false;
	}

	if (programOptions.waterLevel == 0xFF) {
		printf("Please set waterLevel");
		return false;
	}
	if(programOptions.waterLevel > 100)
	{
		programOptions.waterLevel = 100;
	}

	//calculate duration
	uint32_t duration = 600 * 1000;

	bool prewash = programNumber == Wash1Program;
	if(prewash)
	{
		duration += stage_wash_get_duration(20);
	}
	duration += stage_wash_get_duration(programOptions.delay);
	duration += stage_rinsing_get_duration(programOptions.rinsingCycles);
	duration += stage_spinning_get_duration(programOptions.spinningSpeed);

	status_set_program(programNumber, duration);

	//prewash
	printf("Start washing at t: %u, time: %u, washing speed: %u, spinning speed: %u, water level: %u, rinsing cycles: %u, prewash: %s\n",
			programOptions.temperature,
			programOptions.delay,
			programOptions.washingSpeed,
			programOptions.spinningSpeed,
			programOptions.waterLevel,
			programOptions.rinsingCycles,
			prewash ? "true" : "false");

	if(!stage_door_close())
		return false;

	if(prewash)
	{
		status_set_stage(STATUS_PREWASHING);
		if(!stage_wash(programOptions.temperature, 20, programOptions.washingSpeed, prewash_valve, programOptions.waterLevel))
			return false;
	}

	status_set_stage(STATUS_WASHING);
	if(!stage_wash(programOptions.temperature, programOptions.delay, programOptions.washingSpeed, conditioner_valve, programOptions.waterLevel))
		return false;

	if(!stage_rinsing(programOptions.rinsingCycles, programOptions.washingSpeed, programOptions.waterLevel))
		return false;

	if(!stage_spinning(programOptions.spinningSpeed))
		return false;

	if(!stage_door_open())
		return false;

	return true;
}

