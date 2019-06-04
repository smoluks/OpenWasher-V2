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

inline int16_t pid_process(int8_t tacho_currentrps, int8_t tacho_targetrps)
{
	int32_t out = eeprom_config.enginestartvalue;
	int32_t delta = (int32_t)tacho_targetrps - (int32_t)tacho_currentrps;

	//p
	int32_t pterm = delta * P;
	out += pterm;

	//i
	istate += delta;
	if(istate > IMAX)
		istate = IMAX;
	if(istate < IMIN)
		istate = IMIN;
	out += istate * I;

	//d
	out -= (tacho_currentrps - dstate) * D;
	dstate = tacho_currentrps;

	if (out < 0)
		out = 0;
	else if (out > VALUE_FULL)
		out = VALUE_FULL;

	return (int16_t)out;
}

