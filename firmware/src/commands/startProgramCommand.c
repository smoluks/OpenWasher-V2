/*
 * startprogram.c
 *
 *  Created on: 20 но€б. 2019 г.
 *      Author: Shironeko
 */
#include <stdio.h>
#include "stm32f10x.h"
#include "limits.h"
#include "status.h"
#include "action.h"
#include "commandsRoutine.h"
#include "programOptions.h"

program programFromCommand;
programOptions optionsFromCommand;

extern Status currentStatus;
extern uint8_t action;
extern const programOptions defaultProgramOptions[PROGRAM_COUNT];

void processStartProgramCommand(uint8_t* buffer, uint8_t count)
{
	if(currentStatus.program != NoProgram){
		send_answer(startProgramPacketType, NOTREADY);
		printf("Another program started\n");
		return;
	}
	if(count < 1){
		send_answer(startProgramPacketType, BADARGS);
		printf("C %u\n", count);
		return;
	}

	//copy params
	programFromCommand = buffer[0];
	optionsFromCommand = defaultProgramOptions[programFromCommand];
	for(uint8_t i = 0; i < count - 1;i++)
	{
		if(buffer[i+1] != 0xFF)
			((uint8_t* )&optionsFromCommand)[i] = buffer[i+1];
	}

	//validation
	if (programFromCommand >= PROGRAM_COUNT) {
		send_answer(startProgramPacketType, BADARGS);
		printf("select program %u, but program count%u\n", programFromCommand, PROGRAM_COUNT);
		return;
	}
	if (optionsFromCommand.washingSpeed
			!= 0xFF && optionsFromCommand.washingSpeed > MAX_WASHING_SPEED) {
		send_answer(startProgramPacketType, BADARGS);
		printf("select washing speed %u, but max %u\n", optionsFromCommand.washingSpeed, MAX_WASHING_SPEED);
		return;
	}
	if (optionsFromCommand.spinningSpeed
			!= 0xFF && optionsFromCommand.spinningSpeed > MAX_SPINNING_SPEED) {
		send_answer(startProgramPacketType, BADARGS);
		printf("select spinning speed %u, but max %u\n", optionsFromCommand.spinningSpeed, MAX_SPINNING_SPEED);
		return;
	}
	if (optionsFromCommand.temperature
			!= 0xFF && optionsFromCommand.temperature > MAX_TEMPERATURE) {
		send_answer(startProgramPacketType, BADARGS);
		printf("select temperature %u, but max %u\n", optionsFromCommand.temperature, MAX_TEMPERATURE);
		return;
	}
	if (optionsFromCommand.waterLevel
				!= 0xFF && optionsFromCommand.waterLevel > 100) {
			send_answer(startProgramPacketType, BADARGS);
			printf("select water level %u, but max 100\n", optionsFromCommand.waterLevel);
			return;
		}

	action |= ACTION_STARTPROGRAM;
	send_answer(startProgramPacketType, NOEEROR);
}


