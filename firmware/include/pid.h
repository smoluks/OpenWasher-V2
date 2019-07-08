#pragma once

#include "stm32f10x.h"

#define P (2 << 4)
#define I 20
#define D (1 << 4)
#define IMAX (100 << 8)
#define IMIN 0

void pid_clearstate();
uint16_t pid_process(int32_t current, int32_t target);
