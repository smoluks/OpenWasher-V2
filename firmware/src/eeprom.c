#include "i2cHardware.h"
#include "stm32f10x.h"
#include "eeprom.h"
#include "error.h"
#include "delay.h"
#include <string.h>
#include <stdbool.h>
#include <stdio.h>

#define EEPROM_ADDRESS 0b10101110

void setdefaultconfig();
uint8_t calculateCrc(uint8_t* buffer, uint8_t size);

void readconfig()
{
	I2CReadBuffer(EEPROM_ADDRESS, 0, (uint8_t*)&eeprom_config, sizeof(eeprom_config));
	if (eeprom_config.writemarker != 0xAB)
	{
		set_warning(EEPROM_EMPTY);
		setdefaultconfig();
		writeconfig();
		return;
	}

	if (calculateCrc((uint8_t*)&eeprom_config, sizeof(eeprom_config)))
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

bool writeconfig()
{
	eeprom_config.writemarker = 0xAB;
	eeprom_config.crc = calculateCrc((uint8_t*)&eeprom_config, sizeof(eeprom_config)-1);

	if(!I2CWriteBuffer(EEPROM_ADDRESS, 0, (uint8_t*)&eeprom_config, sizeof(eeprom_config)))
	{
		set_error(EEPROM_WRITEERROR);
		return false;
	}

	struct eeprom_config_s readdata;

	I2CReadBuffer(EEPROM_ADDRESS, 0, (uint8_t*)&readdata, sizeof(eeprom_config));

	int cmpresult = memcmp((uint8_t*) &eeprom_config, &readdata, sizeof(eeprom_config));
	if(cmpresult != 0)
	{
		set_error(EEPROM_LOCK);
		return false;
	}

	return true;
}

void setdefaultconfig()
{
	eeprom_config.tachodetectlevel = 10;
}

uint8_t calculateCrc(uint8_t* buffer, uint8_t size)
{
	uint8_t crc = 0;
	while (size--)
	{
		uint8_t data = *(buffer++);
		for (uint8_t p = 0; p < 8; p++)
		{
			if ((crc ^ data) & 0x01)
				crc = ((crc >> 1) ^ 0x8C);
			else
				crc >>= 1;

			data >>= 1;
		}
	}
	return crc;
}

