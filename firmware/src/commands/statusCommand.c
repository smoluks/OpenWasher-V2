/*
 * startprogram.c
 *
 *  Created on: 20 но€б. 2019 г.
 *      Author: Shironeko
 */
#include "stm32f10x.h"
#include "programOptions.h"
#include "limits.h"
#include "status.h"
#include "commandsRoutine.h"

extern Status currentStatus;

void processStatusCommand(__attribute__((unused)) uint8_t* buffer,__attribute__((unused)) uint8_t count)
{
	send_answer_with_data(getStatusPacketType, buildCurrentStatus(), currentStatus.program != NoProgram ? sizeof(Status) : sizeof(Status) - 8);
}


