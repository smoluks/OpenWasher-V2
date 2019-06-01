/*
 * i2c.c
 *
 *  Created on: Feb 8, 2019
 *      Author: Shironeko
 */

#include "stm32f10x.h"
#include "i2c_hardware.h"
#include "main.h"
#include <stdbool.h>

uint8_t I2C_Read(uint8_t reg_addr)
{
	uint8_t data;
	//стартуем
	I2C2->CR1 |= I2C_CR1_START;
	while(!(I2C2->SR1 & I2C_SR1_SB)){ WDT_RESET; };
	(void) I2C2->SR1;

	//передаем адрес устройства
	I2C2->DR = ADDR_WRITE;
	while(!(I2C2->SR1 & I2C_SR1_ADDR)){ WDT_RESET; };
	(void) I2C2->SR1;
	(void) I2C2->SR2;

	//передаем адрес регистра
	I2C2->DR = reg_addr;
	while(!(I2C2->SR1 & I2C_SR1_TXE)){ WDT_RESET; };
	I2C2->CR1 |= I2C_CR1_STOP;

	//рестарт!!!
	I2C2->CR1 |= I2C_CR1_START;
	while(!(I2C2->SR1 & I2C_SR1_SB)){ WDT_RESET; };
	(void) I2C2->SR1;

	//передаем адрес устройства, но теперь для чтения
	I2C2->DR = ADDR_READ;
	while(!(I2C2->SR1 & I2C_SR1_ADDR)){ WDT_RESET; };
	(void) I2C2->SR1;
	(void) I2C2->SR2;

	//читаем
	I2C2->CR1 &= ~I2C_CR1_ACK;
	while(!(I2C2->SR1 & I2C_SR1_RXNE)){ WDT_RESET; };
	data = I2C2->DR;
	I2C2->CR1 |= I2C_CR1_STOP;

	return data;
}

bool I2C_ReadAll(uint8_t* buffer, int size)
{
	uint8_t count = 0;
	while (!I2C_ReadBuffer(buffer, size) && ++count < 10)
		delay_ms(5u);
	if (count == 10)
		return false;

	return true;
}

bool I2C_ReadBuffer(uint8_t* buffer, int size)
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
	I2C2->SR1;

	//передаем адрес устройства
	I2C2->DR = ADDR_WRITE;
	while(!(I2C2->SR1 & (I2C_SR1_ADDR | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	I2C2->SR1;
	I2C2->SR2;

	//передаем адрес регистра
	I2C2->DR = 0;
	while(!(I2C2->SR1 & (I2C_SR1_TXE | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	I2C2->CR1 |= I2C_CR1_STOP;

	//рестарт!!!
	I2C2->CR1 |= I2C_CR1_START;
	while(!(I2C2->SR1 & (I2C_SR1_SB | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	I2C2->SR1;

	//передаем адрес устройства, но теперь для чтения
	I2C2->DR = ADDR_READ;
	while(!(I2C2->SR1 & (I2C_SR1_ADDR | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	(void) I2C2->SR1;
	(void) I2C2->SR2;

	//читаем
	while(size)
	{
		WDT_RESET;
		if(size == 1)
			I2C2->CR1 &= ~I2C_CR1_ACK;
		else
			I2C2->CR1 |= I2C_CR1_ACK;

		while(!(I2C2->SR1 & (I2C_SR1_RXNE | I2C_SR1_AF))){ WDT_RESET; };
		if(I2C2->SR1 & I2C_SR1_AF)
		{
			I2C2->CR1 |= I2C_CR1_STOP;
			return false;
		}
		*buffer++ = I2C2->DR;

		size--;
	}
	I2C2->CR1 |= I2C_CR1_STOP;

	return true;
}


bool I2C_WriteAll(uint8_t* buffer, int size)
{
	for(int i = 0; i < size / PAGESIZE; i++)
	{
		uint8_t count = 0;
		do
		{
			delay_ms(10u);
		}
		while(!I2C_WritePage(i, buffer + i*PAGESIZE) && ++count < 10);

		if(count == 10)
			return false;
	}

	return true;
}

bool I2C_WritePage(uint8_t page, uint8_t* buffer)
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
	I2C2->DR = ADDR_WRITE;
	while(!(I2C2->SR1 & (I2C_SR1_ADDR | I2C_SR1_AF))){ WDT_RESET; };
	if(I2C2->SR1 & I2C_SR1_AF)
	{
		I2C2->CR1 |= I2C_CR1_STOP;
		return false;
	}
	(void) I2C2->SR1;
	(void) I2C2->SR2;

	//передаем адрес регистра
	I2C2->DR = page * PAGESIZE;
	while(!(I2C2->SR1 & I2C_SR1_TXE)){ WDT_RESET; };

	for(uint8_t i = 0; i < PAGESIZE; i++)
	{
		I2C2->DR = *buffer++;
		while (!(I2C2->SR1 & (I2C_SR1_BTF | I2C_SR1_AF))){ WDT_RESET; };
		if(I2C2->SR1 & I2C_SR1_AF)
		{
			continue;
		}
	}

	I2C2->CR1 |= I2C_CR1_STOP;

	return !(I2C2->SR1 & I2C_SR1_AF);
}


