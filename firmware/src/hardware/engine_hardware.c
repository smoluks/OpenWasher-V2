#include "stm32f10x.h"
#include "delay.h"
#include "engine_hardware.h"
#include "led_hardware.h"
#include <stdbool.h>

inline void engine_setdirectioncw() {
	GPIOA->BSRR = 0x00000100;
	GPIOB->BSRR = 0x20000000;
	delay_ms(100);
}

inline void engine_setdirectionccw() {
	GPIOA->BSRR = 0x01000000;
	GPIOB->BSRR = 0x00002000;
	delay_ms(100);
}

inline void engine_setdirectionoff() {
	GPIOA->BSRR = 0x01000000;
	GPIOB->BSRR = 0x20000000;
	delay_ms(100);
}

void engine_emergencyoff(){
	GPIOB->BSRR = 0x00010000;
	GPIOA->BSRR = 0x01000000;
	delay_ms(10);
	GPIOB->BSRR = 0x20000000;
}

inline enum direction_e get_direction() {
	if ((GPIOA->ODR & 0x100) && !(GPIOB->ODR & 0x2000))
		return cw;
	else if (!(GPIOA->ODR & 0x100) && (GPIOB->ODR & 0x2000))
		return ccw;
	else return off;
}

inline void engine_triakon() {
	GPIOB->BSRR = 0x00000001;
}

inline void engine_triakoff() {
	GPIOB->BSRR = 0x00010000;
}
