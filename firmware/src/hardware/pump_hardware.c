#include "stm32f10x.h"
#include <stdbool.h>

void pump_relayenable()
{
	GPIOB->BSRR = 0x00008000;
}

void pump_relaydisable()
{
	GPIOB->BSRR = 0x80000000;
}

bool pump_isrelayenable()
{
	return GPIOB->ODR & 0x8000;
}

void pump_triakenable()
{
	GPIOB->BSRR = 0x00000020;
}

void pump_triakdisable()
{
	GPIOB->BSRR = 0x00200000;
}


