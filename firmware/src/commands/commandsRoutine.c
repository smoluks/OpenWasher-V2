/*
 * commandsRoutine.c
 *
 *  Created on: 24 но€б. 2019 г.
 *      Author: Shironeko
 */

#include "commandsRoutine.h"
#include "uart_driver.h"

void send_empty_answer()
{
	send(0xAB);
	send(0x00);
	send(0xCD);
}

void send_answer(commandType command, commandError state)
{
	uint8_t crc;
	//Start token
	send(0xAB);
	//Length
	send(1);
	//
	crc = addcrc(0x93, command | state);
	send(command | state);
	//
	send(crc);
}

void send_answer_with_data(commandType command, uint8_t* data, uint8_t length)
{
	uint8_t crc;
	//Start token
	send(0xAB);
	//Length
	crc = addcrc(0x8F, length+1);
	send(length+1);
	//
	crc = addcrc(crc, command);
	send(command);
	//
	for(int i = 0; i < length; i++)
	{
		crc = addcrc(crc, data[i]);
		send(data[i]);
	}
	//
	send(crc);
}


