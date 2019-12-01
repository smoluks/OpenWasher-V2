/*
 * clock.c
 *
 *  Created on: 30 но€б. 2019 г.
 *      Author: Shironeko
 */

#include "i2cHardware.h"
#include "clock.h"

#define CLOCK_ADDRESS 0b11011110

void initClock()
{
	uint8_t sec = I2CReadRegister(CLOCK_ADDRESS, RTCSEC);
	if(!(sec & RTCSEC_ST))
		I2CWriteRegister(CLOCK_ADDRESS, RTCSEC, sec | RTCSEC_ST);

	//TODO: clock crystal fail
	uint8_t weekday = I2CReadRegister(CLOCK_ADDRESS, RTCWKDAY);
	if(!(weekday & RTCWKDAY_VBATEN))
		I2CWriteRegister(CLOCK_ADDRESS, RTCWKDAY, weekday | RTCWKDAY_VBATEN);
}
