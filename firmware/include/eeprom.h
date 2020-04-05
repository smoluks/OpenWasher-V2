#pragma once
#include <stdbool.h>
#include "stm32f10x.h"

void readconfig();
bool writeconfig();

#pragma pack(push, 1)

struct eeprom_config_s
{
	uint8_t writemarker;
	//
	uint16_t enginestartvalue;
	//
	uint16_t tachodetectlevel;
	//
	uint16_t temperaturenoise;
	//
	uint32_t waterleveldowntime;
	//
	uint32_t waterleveluptime;

	uint8_t crc;
} eeprom_config;

#pragma pack(pop)
