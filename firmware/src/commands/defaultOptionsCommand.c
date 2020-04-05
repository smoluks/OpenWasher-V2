/*
 * startprogram.c
 *
 *  Created on: 20 но€б. 2019 г.
 *      Author: Shironeko
 */
#include "stm32f10x.h"
#include "programs.h"
#include "commandsRoutine.h"
#include "programOptions.h"

extern const programOptions defaultProgramOptions[PROGRAM_COUNT];

void processDefaultOptionsCommand(uint8_t* buffer, uint8_t count)
{
	if (count < 1) {
		send_answer(getProgramDefaultsPacketType, BADARGS);
		return;
	}

	uint8_t program = buffer[0];
	if (program >= PROGRAM_COUNT) {
		send_answer(getProgramDefaultsPacketType, BADARGS);
		return;
	}

	send_answer_with_data(getProgramDefaultsPacketType, (uint8_t*)&(defaultProgramOptions[program]), sizeof(programOptions));
}


