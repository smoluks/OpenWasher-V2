/*
 * hc05_uart_driver.c
 *
 *  Created on: Feb 4, 2019
 *      Author: Shironeko
 */

#include "stm32f10x.h"
#include "delay.h"
#include "main.h"
#include <stdbool.h>

void hc05_sendbyte(char b);
char hc05_readbyte();

void hc05_enter_setting_mode()
{
	NVIC_DisableIRQ(USART1_IRQn);
	GPIOB->BSRR = 0x02000000; //reset
	delay_ms(100u);
	GPIOB->BSRR = 0x00000300; //set command mode
	delay_ms(5000u);
	USART1->BRR = 1875;
	USART1->CR1 = USART_CR1_UE | USART_CR1_TE | USART_CR1_RE;
}

void hc05_leave_setting_mode()
{
	GPIOB->BSRR = 0x02000000; //reset
	delay_ms(100u);
	GPIOB->BSRR = 0x03000200; //clear command mode
	delay_ms(5000u);
	USART1->BRR = 0x271;
	USART1->CR1 = USART_CR1_UE | USART_CR1_TE | USART_CR1_RE | USART_CR1_PCE  | USART_CR1_M | USART_CR1_RXNEIE | USART_CR1_IDLEIE;
	NVIC_EnableIRQ(USART1_IRQn);
}

char buffer[50];
char* hc05_sendcommand(char* data)
{
	char c;
	do
	{
		c = *data++;
		if(!c)
			break;
		hc05_sendbyte(c);
	}
	while(true);

	hc05_sendbyte(0x0D);
	hc05_sendbyte(0x0A);
	USART1->DR;
	uint32_t timestamp = get_systime() + 3000u;

	char* handle = buffer;
	do {
		WDT_RESET;

		c = hc05_readbyte();
		if(c == -1)
			continue;

		if (c == 0x0D || c == 0x0A)
		{
			*handle = 0;
			return buffer;
		}
		*handle++ = c;

	} while (!check_time_passed(timestamp));
	return 0;
}

void hc05_sendbyte(char b)
{
	while(!(USART1->SR & USART_SR_TXE));

	USART1->DR = b;
}

char hc05_readbyte()
{
	if(!(USART1->SR & USART_SR_RXNE))
		return -1;

	return USART1->DR;
}
