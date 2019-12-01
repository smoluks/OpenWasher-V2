/*
 * i2c.h
 *
 *  Created on: Feb 8, 2019
 *      Author: Shironeko
 */

#pragma once

#include "stm32f10x.h"
#include <stdbool.h>

#define PAGESIZE 8

uint8_t I2CReadRegister(uint8_t devAddress, uint8_t registerAddress);
bool I2CReadBuffer(uint8_t devAddress, uint8_t startAddress, uint8_t* buffer, int size);
bool I2CWriteRegister(uint8_t devAddress, uint8_t registerAddress, uint8_t value);
bool I2CWritePage(uint8_t devAddress, uint8_t registerAddress, uint8_t* buffer, uint8_t count);
bool I2CWriteBuffer(uint8_t devAddress, uint8_t startAddress, uint8_t* buffer, int size);
