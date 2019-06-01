#pragma once

enum errorcode
{
	NOERROR = 0x0,						// нет ошибки
	CRYSTAL =  0x1,						// не удалось запустить кварцевый резонатор
	PLL =  0x2,							// не удалось запустить PLL
	SW_PLL =  0x3,						// не удалось переключиться на PLL
	LSI =  0x4,							// не удалось запустить LSI
	HardFault =  0x5,					// критическая ошибка
	MemManageFault =  0x6,				// ошибка системы управления памятью
	BusFault =  0x7,					// ошибка шины
	UsageFault =  0x8,					// ошибка программы
	WDTRESET =  0x9,					// перезагрузка по watchdog
	//насос
	BAD_PUMP_RELAY =  0x10, 			// залипание реле насоса
	BAD_PUMP =  0x11,					// нет фидбека насоса
	BAD_PUMP_TRIAK =  0x12,				// есть фидбэк при включенном насосе
	BAD_PUMP_TRIAK2 =  0x13,			// нет фидбэка при выключенном насосе
	//прессостат
	WATERLEVEL_UP_ON =  0x20,			// залип верхний датчик давления
	WATERLEVEL_DOWN_ON =  0x21,			// залип нижний датчик давления
	WATERLEVEL_UP_OFF =  0x22,			// не срабатывает верхний датчик давления
	WATERLEVEL_DOWN_OFF =  0x23,		// не срабатывает  нижний датчик давления
	OVERFLOW =  0x24,					// переполнение
	//тэн
	OVERHEAT =  0x30,					// температура выше 95
	NTC_NOT_PRESENT =  0x31,			// нет сигнала с датчика температуры
	NO_HEATER =  0x32,					// нет нагрева воды
	BAD_HEATER_RELAY =  0x33,			// нет фидбека тэна
	HEATER_RELAY_STICKING =  0x34,		// реле тэна залипло
	TRY_SET_HEAT_WITHOUT_WATER =  0x35, // включение нагрева без воды
	SET_HEAT_OVER90 =  0x36, 			// попытка выставить температуру выше 90
	//двигатель
	NO_ENGINE =  0x40,					// нет фидбека двигателя
	ENGINE_RELAYCW_STICKING =  0x41,	// залипание реле двигателя 1
	ENGINE_RELAYCCW_STICKING =  0x42,	// залипание реле двигателя 2
	NO_TACHO =  0x43,					// нет сигнала с тахометра или двигатель не раскручивается
	PID_ERROR =  0x44,					//
	//дверца
	NO_LOCKER = 0x50,					// нет сигнала от защелки
	BAD_DOOR_TRIAK = 0x51,				// нет фидбэка от защелки
	BAD_DOOR_TRIAK2 = 0x52,				// есть фидбэк от защелки при отключенном симисторе
	TRY_OPEN_DOOR_WITH_WATER =  0x53,   // попытка разблокировать дверцу при набранной воде
	//клапаны
	TRY_OPEN_WATER_WITH_OPEN_DOOR =  0x60,	// попытка набрать воду при открытой дверце
	TRY_OPEN_WATER_WITH_WATER =  0x61,	// попытка набрать воду при набранной воде
	//eeprom
	EEPROM_EMPTY =  0x70,				// нет маркера записи
	EEPROM_BADCRC =  0x71,				// неправильный crc
	EEPROM_LOCK =  0x72,				// записанные данные не совпадают с записываемыми
	EEPROM_WRITEERROR =  0x73,			// не удалось записать флешкy
};

void set_error(enum errorcode code);
void set_warning(enum errorcode code);

