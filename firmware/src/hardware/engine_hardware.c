#include "stm32f10x.h"
#include "delay.h"
#include "engine_hardware.h"
#include "led_hardware.h"
#include <stdbool.h>

inline void engine_setdirectioncw() {
	GPIOA->BSRR = 0x00001000;
	GPIOB->BSRR = 0x00400000;
	delay_ms(100);
}

inline void engine_setdirectionccw() {
	GPIOA->BSRR = 0x10000000;
	GPIOB->BSRR = 0x00000040;
	delay_ms(100);
}

inline void engine_setdirectionoff() {
	GPIOA->BSRR = 0x10000000;
	GPIOB->BSRR = 0x00400000;
	delay_ms(100);
}


inline enum direction_e get_direction() {
	if ((GPIOA->ODR & 0x1000) && !(GPIOB->ODR & 0x0040))
		return cw;
	else if (!(GPIOA->ODR & 0x1000) && (GPIOB->ODR & 0x0040))
		return ccw;
	else return off;
}

inline void engine_triakon() {
	GPIOA->BSRR = 0x00000080;
}

inline void engine_triakoff() {
	GPIOA->BSRR = 0x00800000;
}
