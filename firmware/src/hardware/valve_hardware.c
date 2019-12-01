#include "stm32f10x.h"
#include "valve_hardware.h"
#include "delay.h"
#include <stdbool.h>
#include <valve_driver.h>

volatile enum valve_state valve_prewash_state = valve_closed;
volatile enum valve_state valve_wash_state = valve_closed;

bool is_water()
{
	return GPIOB->IDR & 0x00000004;
}

bool is_overflow()
{
	return GPIOA->IDR & 0x00000008;
}

void valve_emergencyclose()
{
	GPIOB->BSRR = 0x00500000;
}

void valve_crosszero(bool phase)
{
	if((valve_prewash_state == valve_opened) || ((valve_prewash_state == valve_retention) && phase))
		GPIOB->BSRR = 0x00000010;
	else
		GPIOB->BSRR = 0x00100000;

	if((valve_wash_state == valve_opened) || ((valve_wash_state == valve_retention) && phase))
		GPIOB->BSRR = 0x00000040;
	else
		GPIOB->BSRR = 0x00400000;

}
