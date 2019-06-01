/*
 * led_hardware.c
 *
 *  Created on: Feb 5, 2019
 *      Author: Shironeko
 */
#include "stm32f10x.h"
#include "led_hardware.h"
#include <stdbool.h>

inline void led_orange_on()
{
	GPIOC->BSRR = 0x4000;
}

inline void led_orange_off()
{
	GPIOC->BSRR = 0x40000000;
}

inline bool get_led_orange_state()
{
	return GPIOC->ODR & 0x4000;
}

inline void led_green_on()
{
	GPIOC->BSRR = 0x2000;
}

inline void led_green_off()
{
	GPIOC->BSRR = 0x20000000;
}

inline bool get_led_green_state()
{
	return GPIOC->ODR & 0x2000;
}
