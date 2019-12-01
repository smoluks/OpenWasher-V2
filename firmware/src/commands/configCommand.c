/*
 * startprogram.c
 *
 *  Created on: 20 но€б. 2019 г.
 *      Author: Shironeko
 */
#include <string.h>
#include "stm32f10x.h"
#include "eeprom.h"
#include "commandsRoutine.h"

void processConfigCommand(uint8_t* buffer, uint8_t count)
{
	if(count > 1)
	{
		//Write
		if(count != sizeof(eeprom_config)+1)
			send_answer(setSetConfigPacketType, BADARGS);

		memcpy(&eeprom_config, buffer+1, sizeof(eeprom_config));
		send_answer(setSetConfigPacketType, NOEEROR);
	}
	else
	{
		//Read
		send_answer_with_data(setSetConfigPacketType, (uint8_t*)&eeprom_config, sizeof(eeprom_config));
	}
}


