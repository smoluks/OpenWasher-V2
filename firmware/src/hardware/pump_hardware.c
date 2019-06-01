#include "stm32f10x.h"
#include <stdbool.h>

void pump_relayenable()
{
	GPIOA->BSRR = 0x00000800;
}

void pump_relaydisable()
{
	GPIOA->BSRR = 0x08000000;
}

bool pump_isrelayenable()
{
	return GPIOA->ODR & 0x0800;
}

void pump_triakenable()
{
	GPIOB->BSRR = 0x00004000;
}

void pump_triakdisable()
{
	GPIOB->BSRR = 0x40000000;
}


