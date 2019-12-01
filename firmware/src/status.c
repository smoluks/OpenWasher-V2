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

Status currentStatus = {NoProgram, 0, 0, 0, 0};
uint32_t startProgramTimestamp;

inline uint8_t* buildCurrentStatus()
{
	currentStatus.temperature = get_temperature();
	return (uint8_t*)&currentStatus;
}


inline void status_set_program(uint8_t program, uint32_t fullTimeLength)
{
	currentStatus.program = program;
	getsystime(&startProgramTimestamp);
	currentStatus.timefull = fullTimeLength;
}

inline void status_set_stage(enum stage_e stage)
{
	currentStatus.stage = stage;
}

