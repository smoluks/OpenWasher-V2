#pragma once

#include "stm32f10x.h"
#include "options.h"

void processcommand(uint8_t* buffer, uint8_t count);

enum commanderr_e
{
	NOEEROR = 0,
	UNSUPPORTEDCOMMAND = 0x40,
	BADARGS = 0x80,
	NOTREADY = 0xC0,
};

volatile uint8_t action;
volatile options actionargs;
