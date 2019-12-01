/*
 * startprogram.c
 *
 *  Created on: 20 но€б. 2019 г.
 *      Author: Shironeko
 */
#include <stdbool.h>
#include "stm32f10x.h"
#include "action.h"
#include "commandsRoutine.h"

extern uint8_t action;
extern volatile bool ct;

void processGoToBootloaderModeCommand(__attribute__((unused)) uint8_t* buffer,__attribute__((unused)) uint8_t count)
{
	ct = true;
	send_answer(0x0A, NOEEROR);

	action |= ACTION_GOTOBOOTLOADER;
}


