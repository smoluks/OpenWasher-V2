/*
 * hc05Hardware.c
 *
 *  Created on: 26 но€б. 2019 г.
 *      Author: Shironeko
 */

#include <stdbool.h>
#include "stm32f10x.h"

void hc05ResetToCommandMode()
{
	GPIOA->BSRR = 0x10000800;
	delay_ms(100u);
	GPIOA->BSRR = 0x00001000;
	delay_ms(5000u);
}

void hc05ResetToNormalMode()
{
	GPIOA->BSRR = 0x18000000;
	delay_ms(100u);
	GPIOA->BSRR = 0x00001000;
	delay_ms(5000u);
}

