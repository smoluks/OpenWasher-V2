#include "stm32f10x.h"

//TODO: не открывать в отрицательной полуволне - там диод
void lock_door()
{
	GPIOA->BSRR = 0x00000100;
}


void unlock_door()
{
	GPIOA->BSRR = 0x01000000;
}

