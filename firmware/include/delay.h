#pragma once
#include <stdbool.h>
#include <stdint-gcc.h>

#define uS 9

void delay_us(uint16_t us);
void delay_ms(uint32_t ms);
void delay_ms_with_ct(uint32_t ms);
