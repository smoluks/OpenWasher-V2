/*
 * pid.c
 *
 *  Created on: 3 θών. 2019 γ.
 *      Author: Shironeko
 */

#include "pid.h"
#include "engine_driver.h"
#include "eeprom.h"

volatile int32_t istate = 0;
volatile int32_t dstate = 0;

void pid_clearstate()
{
	istate = 0;
	dstate = 0;
}

uint16_t pid_process(int32_t current, int32_t target)
{
	int32_t out = (48 + (target * 2)) << 8;
	int32_t delta = target - current;

	//p
	out += delta * P;

	//i
	istate += delta * I;
	if(istate > IMAX)
		istate = IMAX;
	else if(istate < IMIN)
		istate = IMIN;
	out += istate;

	//d
	out -= (current - dstate) * D;
	dstate = current;

	if (out <= 0)
		return 0;
	else if (out >= (VALUE_FULL << 8))
		return VALUE_FULL;
	else
		return out >> 8;
}

