#include "stm32f10x.h"
#include "eeprom.h"
#include "error.h"
#include "delay.h"
#include "i2c_hardware.h"
#include <string.h>
#include <stdbool.h>
#include <stdio.h>

void setdefaultconfig();

void readconfig()
{
	I2C_TryReadBuffer((uint8_t*)&eeprom_config, sizeof(eeprom_config));
	if (eeprom_config.writemarker != 0xAB)
	{
		set_warning(EEPROM_EMPTY);
		setdefaultconfig();
		writeconfig();
		return;
	}

	uint8_t crc = 0;
	uint8_t* config = (uint8_t*)&eeprom_config;
	for (uint32_t i = 0; i < sizeof(eeprom_config); i++)
	{
		uint8_t data = config[i];
		for (uint8_t p = 0; p < 8; p++)
		{
			if ((crc ^ data) & 0x01)
				crc = ((crc >> 1) ^ 0x8C);
			else
				crc >>= 1;

			data >>= 1;
		}
	}
	if (crc)
	{
		set_warning(EEPROM_BADCRC);
		setdefaultconfig();
		writeconfig();
		return;
	}

	if(eeprom_config.tachodetectlevel<10 || eeprom_config.tachodetectlevel == 0xFFFF)
	{
		set_warning(EEPROM_BADCRC);
		setdefaultconfig();
		writeconfig();
	}
}

void writeconfig()
{
	eeprom_config.writemarker = 0xAB;

	uint8_t crc = 0;
	uint8_t* config = (uint8_t*) &eeprom_config;
	for (uint32_t i = 0; i < sizeof(eeprom_config)-1; i++)
	{
		uint8_t data = config[i];
		for (uint8_t p = 0; p < 8; p++)
		{
			if ((crc ^ data) & 0x01)
				crc = ((crc >> 1) ^ 0x8C);
			else
				crc >>= 1;

			data >>= 1;
		}
	}
	eeprom_config.crc = crc;

	if(!I2C_WriteBuffer((uint8_t*)&eeprom_config, sizeof(eeprom_config)))
	{
		set_error(EEPROM_WRITEERROR);
		return;
	}

	uint8_t readdata[sizeof(eeprom_config)];

	I2C_TryReadBuffer((uint8_t*)readdata, sizeof(eeprom_config));

	int cmpresult = memcmp((uint8_t*) &eeprom_config, readdata, sizeof(eeprom_config));
	if(cmpresult != 0)
		set_error(EEPROM_LOCK);
}

void setdefaultconfig()
{
	eeprom_config.tachodetectlevel = 10;
}
