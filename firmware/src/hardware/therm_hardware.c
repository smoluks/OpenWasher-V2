#include <stdbool.h>
#include <stm32f10x.h>
#include "therm_hardware.h"

inline void heater_enable()
{
	GPIOB->BSRR = 0x00004000;
}

inline void heater_disable()
{
	GPIOB->BSRR = 0x40000000;
}

