#pragma once

#include "stm32f10x.h"

#define P 256
#define I 36
#define D 64
#define IMAX 32737
#define IMIN 0

void pid_clearstate();
int16_t pid_process(int8_t tacho_currentrps, int8_t tacho_targetrps);
