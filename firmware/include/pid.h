#pragma once

#include "stm32f10x.h"

#define P 15
#define I_ACCS 16
#define I_BRAK 1
#define D 256

void pid_clearstate();
void pid_setstate(uint16_t i);
uint16_t pid_process(int32_t currentSpeed, int32_t targetSpeed);
