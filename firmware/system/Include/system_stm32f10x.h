#pragma once

#include <stdint.h>
#include "error.h"

#define nop() asm("NOP")

volatile enum errorcode initError;

void SystemInit(void);
enum errorcode SystemCoreClockUpdate();
