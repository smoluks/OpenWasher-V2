/*
 * status.c
 *
 *  Created on: Apr 11, 2019
 *      Author: Shironeko
 */

#include "stm32f10x.h"
#include "status.h"
#include "therm_driver.h"
#include "programs.h"
#include "systick.h"

Status currentStatus = {NoProgram, 0, 0, 0, 0, 0, 0};
volatile uint32_t startProgramTimestamp;

extern volatile uint16_t engine_current_speed;
extern enum errorcode error;

inline uint8_t* buildCurrentStatus()
{
	currentStatus.temperature = get_temperature();
	currentStatus.rotationSpeed = engine_current_speed;
	currentStatus.programTimePassed = get_systime() - startProgramTimestamp;
	currentStatus.error = error;
	return (uint8_t*)&currentStatus;
}

inline void status_set_program(uint8_t program, uint32_t fullTimeLength)
{
	currentStatus.program = program;
	currentStatus.programDuration = fullTimeLength;

	startProgramTimestamp = get_systime();
}

inline void status_set_stage(enum stage_e stage)
{
	currentStatus.stage = stage;
}

