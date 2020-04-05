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

Status currentStatus = {NoProgram, 0, 0, 0, 0, 0};
uint32_t startProgramTimestamp;

extern volatile uint16_t engine_current_speed;

inline uint8_t* buildCurrentStatus()
{
	currentStatus.temperature = get_temperature();
	currentStatus.rotationSpeed = engine_current_speed;
	return (uint8_t*)&currentStatus;
}


inline void status_set_program(uint8_t program, uint32_t fullTimeLength)
{
	currentStatus.program = program;
	getsystime(&startProgramTimestamp);
	currentStatus.programDuration = fullTimeLength;
}

inline void status_set_stage(enum stage_e stage)
{
	currentStatus.stage = stage;
}

