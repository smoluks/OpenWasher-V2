#pragma once

#include "stm32f10x.h"

#define P 0
#define I 1
#define D 0
#define IMAX 1020 << 8
#define IMIN 0

void pid_clearstate();
uint16_t pid_process(int32_t currentSpeed, int32_t targetSpeed);
