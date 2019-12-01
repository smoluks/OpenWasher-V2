#pragma once

#include "stm32f10x.h"

typedef enum {
	startProgramPacketType = 0x01,
	stopProgramPacketType = 0x02,
	getProgramDefaultsPacketType = 0x03,
	goToBootloaderModePacketType = 0x0A,
	getStatusPacketType = 0x10,
	setSetConfigPacketType = 0x20,
	getSetTimePacketType = 0x30,
} commandType;

typedef enum {
	NOEEROR = 0,
	UNSUPPORTEDCOMMAND = 0x40,
	BADARGS = 0x80,
	NOTREADY = 0xC0,
} commandError;

void send_empty_answer();
void send_answer(commandType command, commandError error);
void send_answer_with_data(commandType command, uint8_t* data, uint8_t length);



