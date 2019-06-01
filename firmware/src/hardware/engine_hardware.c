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

inline bool engine_isdirectioncw() {
	return GPIOA->ODR & 0x1000;
}

inline bool engine_isdirectionccw() {
	return GPIOB->ODR & 0x0040;
}

inline bool engine_isanydirection() {
	return (GPIOA->ODR & 0x1000) || (GPIOB->ODR & 0x0040);
}

inline void engine_triakon() {
	GPIOA->BSRR = 0x00000080;
	led_orange_on();
}

inline void engine_triakoff() {
	GPIOA->BSRR = 0x00800000;
	led_orange_off();
}
