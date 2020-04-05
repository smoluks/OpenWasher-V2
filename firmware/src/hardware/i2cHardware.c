/*
 * i2c.c
 *
 *  Created on: Feb 8, 2019
 *      Author: Shironeko
 */

#include <stdbool.h>
#include "i2cHardware.h"
#include "stm32f10x.h"
#include "delay.h"
#include "watchdog.h"

uint8_t I2CReadRegister(uint8_t devAddress, uint8_t registerAddress)
{
	uint8_t data;
	//send start
	I2C2->CR1 |= I2C_CR1_START;
	while(!(I2C2->SR1 & I2C_SR1_SB)){ WDT_RESET; };
	(void) I2C2->SR1;

	//send chip address
	I2C2->DR = devAddress & 0xFE;
	while(!(I2C2->SR1 & I2C_SR1_ADDR)){ WDT_RESET; };
	(void) I2C2->SR1;
	(void) I2C2->SR2;

	//send register address
	I2C2->DR = registerAddress;
	while(!(I2C2->SR1 & I2C_SR1_TXE)){ WDT_RESET; };
	I2C2->CR1 |= I2C_CR1_STOP;

	//send repeated start
	I2C2->CR1 |= I2C_CR1_START;
	while(!(I2C2->SR1 & I2C_SR1_SB)){ WDT_RESET; };
	(void) I2C2->SR1;

	//send chip address
	I2C2->DR = devAddress | 0x01;
	while(!(I2C2->SR1 & I2C_SR1_ADDR)){ WDT_RESET; };
	(void) I2C2->SR1;
	(void) I2C2->SR2;

	//read
	I2C2->CR1 &= ~I2C_CR1_ACK;
	while(!(I2C2->SR1 & I2C_SR1_RXNE)){ WDT_RESET; };
	data = I2C2->DR;

	//send stop
	I2C2->CR1 |= I2C_CR1_STOP;

	return data;
}

bool I2CReadBuffer(uint8_t devAddress, uint8_t startAddress, uint8_t* buffer, int size)
{
	bool deviceReady = false;

	do {
		I2C2->SR1 &= ~I2C_SR1_AF;

		//make start
		I2C2->CR1 |= I2C_CR1_START;
		while (!(I2C2->SR1 & I2C_SR1_SB)) {
			WDT_RESET;
		};

		//send device address (write)
		I2C2->DR = devAddress & 0xFE;
		while (!(I2C2->SR1 & (I2C_SR1_ADDR | I2C_SR1_AF))) {
			WDT_RESET;
		};
		if (I2C2->SR1 & I2C_SR1_AF) {
			I2C2->CR1 |= I2C_CR1_STOP;
		} else {
			deviceReady = true;
		}

		I2C2->SR2;
	} while (!deviceReady); //TODO: make count 100

	//send register address
	I2C2->DR = startAddress;
	while(!(I2C2->SR1 & (I2C_SR1_TXE | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}

	//make restart
	I2C2->CR1 |= I2C_CR1_STOP;
	I2C2->CR1 |= I2C_CR1_START;
	while(!(I2C2->SR1 & I2C_SR1_SB)){ WDT_RESET; };

	//send device address (read)
	I2C2->DR = devAddress | 0x01;
	while(!(I2C2->SR1 & (I2C_SR1_ADDR | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	(void) I2C2->SR2;

	uint8_t* bufferptr = buffer;
	//read
	while(size)
	{
		if(size == 1)
			I2C2->CR1 &= ~I2C_CR1_ACK;
		else
			I2C2->CR1 |= I2C_CR1_ACK;

		while(!(I2C2->SR1 & I2C_SR1_RXNE)){ WDT_RESET; };
		*bufferptr++ = I2C2->DR;
		size--;
	}

	I2C2->CR1 |= I2C_CR1_STOP;

	return true;
}

/*bool I2C_TryReadBuffer(uint8_t* buffer, int size)
{
	uint8_t count = 0;
	while (!I2C_ReadBuffer(buffer, size) && ++count < 10)
		delay_ms(5u);
	if (count == 10)
		return false;

	return true;
}*/

bool I2CWritePage(uint8_t devAddress, uint8_t registerAddress, uint8_t* buffer, uint8_t count)
{
	I2C2->SR1 &= ~I2C_SR1_AF;
	bool deviceReady = false;
	uint8_t tryCount = 0;

	do {
		//make start
		I2C2->CR1 |= I2C_CR1_START;
		while (!(I2C2->SR1 & I2C_SR1_SB)) {
			WDT_RESET;
		};

		//send device address
		I2C2->DR = devAddress & 0xFE;
		while (!(I2C2->SR1 & (I2C_SR1_ADDR | I2C_SR1_AF))) {
			WDT_RESET;
		};
		if (I2C2->SR1 & I2C_SR1_AF) {
			tryCount++;
			I2C2->SR1 &= ~I2C_SR1_AF;
			(void) I2C2->SR2;
			I2C2->CR1 |= I2C_CR1_STOP;
		} else {
			(void) I2C2->SR2;
			deviceReady = true;
		}
	} while (!deviceReady); //TODO: make count 100

	//send memory address
	I2C2->DR = registerAddress;
	while(!(I2C2->SR1 & (I2C_SR1_BTF | I2C_SR1_AF))){ WDT_RESET; };
	if (I2C2->SR1 & I2C_SR1_AF) {
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}

	//send data
	for(uint8_t i = 0; i < count; i++)
	{
		I2C2->DR = *(buffer++);
		while (!(I2C2->SR1 & (I2C_SR1_BTF | I2C_SR1_AF))){ WDT_RESET; };
		if(I2C2->SR1 & I2C_SR1_AF)
		{
			continue;
		}
	}

	I2C2->CR1 |= I2C_CR1_STOP;

	return !(I2C2->SR1 & I2C_SR1_AF);
}

bool I2CWriteBuffer(uint8_t devAddress, uint8_t startAddress, uint8_t* buffer, int size)
{
	while(size >= 8)
	{
		if(!I2CWritePage(devAddress, startAddress, buffer, 8))
			return false;

		startAddress +=8;
		buffer+=8;
		size-=8;
	}

	if(size > 0)
		return I2CWritePage(devAddress, startAddress, buffer, size);

	return true;
}

bool I2CWriteRegister(uint8_t devAddress, uint8_t registerAddress, uint8_t value)
{
	I2C2->SR1 &= ~I2C_SR1_AF;
	//стартуем

	I2C2->CR1 |= I2C_CR1_START;
	while(!(I2C2->SR1 & (I2C_SR1_SB | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	(void) I2C2->SR1;

	//передаем адрес устройства
	I2C2->DR = devAddress & 0xFE;
	while(!(I2C2->SR1 & (I2C_SR1_ADDR | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	(void) I2C2->SR1;
	(void) I2C2->SR2;

	//передаем адрес регистра
	I2C2->DR = registerAddress;
	while(!(I2C2->SR1 & I2C_SR1_TXE)){ WDT_RESET; };

	I2C2->DR = value;
	while (!(I2C2->SR1 & (I2C_SR1_BTF | I2C_SR1_AF))){ WDT_RESET; };
	I2C2->CR1 |= I2C_CR1_STOP;

	return !(I2C2->SR1 & I2C_SR1_AF);
}

