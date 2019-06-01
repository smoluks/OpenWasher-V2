/*
 * status.c
 *
 *  Created on: Apr 11, 2019
 *      Author: Shironeko
 */

#include "stm32f10x.h"
#include "status.h"
#include "therm_driver.h"

struct status_s status;

inline uint8_t* get_status()
{
	status.temperature = get_temperature();
	return (uint8_t*)&status;
}


inline void status_set_program(uint8_t program)
{
	status.program = program;
}

inline void status_set_stage(enum stage_e stage)
{
	status.stage = stage;
}


