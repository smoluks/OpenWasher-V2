/*
 * i2c.h
 *
 *  Created on: Feb 8, 2019
 *      Author: Shironeko
 */

#pragma once

#include "stm32f10x.h"
#include <stdbool.h>

#define ADDR_WRITE 0xA0
#define ADDR_READ 0xA1
#define PAGESIZE 16

uint8_t I2C_Read(uint8_t reg_addr);
bool I2C_ReadAll(uint8_t* buffer, int size);
bool I2C_ReadBuffer(uint8_t* buffer, int size);
bool I2C_WritePage(uint8_t page, uint8_t* buffer);
bool I2C_WriteAll(uint8_t* buffer, int size);
