using System.ComponentModel;

namespace OpenWasherHardwareLibrary.Enums
{
    public enum ErrorType
    {
        [Description("No error")]
        NOERROR = 0x0,

        #region System

        [Description("Crystal resonator fault")]
        CRYSTAL = 0x1,                  // не удалось запустить кварцевый резонатор
        [Description("PLL fault")]
        PLL = 0x2,                      // не удалось запустить PLL
        [Description("PLL switch fault")]
        SW_PLL = 0x3,                   // не удалось переключиться на PLL
        [Description("LSI fault")]
        LSI = 0x4,                      // не удалось запустить LSI
        [Description("HardFault")]
        HardFault = 0x5,                // критическая ошибка
        [Description("MemManageFault")]
        MemManageFault = 0x6,           // ошибка системы управления памятью
        [Description("BusFault")]
        BusFault = 0x7,                 // ошибка шины
        [Description("UsageFault")]
        UsageFault = 0x8,               // ошибка программы
        [Description("Watchdog reset")]
        WDTRESET = 0x9,                 // перезагрузка по watchdog
        
        #endregion

        #region Pump

        [Description("Pump relay sticking")]
        BAD_PUMP_RELAY = 0x10,          // залипание реле насоса
        [Description("No pump feedback")]
        BAD_PUMP = 0x11,                // нет фидбека насоса
        [Description("Pump triak break")]
        BAD_PUMP_TRIAK = 0x12,          // есть фидбэк при включенном насосе
        [Description("Pump triak breakdown")]
        BAD_PUMP_TRIAK2 = 0x13,         // нет фидбэка при выключенном насосе

        #endregion

        #region Pressure switch

        [Description("Hi pressure switch sticking")]
        WATERLEVEL_UP_ON = 0x20,        // залип верхний датчик давления
        [Description("Low pressure switch sticking")]
        WATERLEVEL_DOWN_ON = 0x21,      // залип нижний датчик давления
        [Description("Hi pressure switch breakdown")]
        WATERLEVEL_UP_OFF = 0x22,       // не срабатывает верхний датчик давления
        [Description("Low pressure switch breakdown")]
        WATERLEVEL_DOWN_OFF = 0x23,     // не срабатывает  нижний датчик давления
        [Description("Flood!")]
        OVERFLOW = 0x24,                // переполнение

        #endregion

        #region Heater

        [Description("Overheat")]
        OVERHEAT = 0x30,                    // температура выше 95
        [Description("Temperature sensor break")]
        NTC_NOT_PRESENT = 0x31,             // нет сигнала с датчика температуры
        [Description("No heating")]
        NO_HEATER = 0x32,                   // нет нагрева воды
        [Description("Heater break")]
        BAD_HEATER_RELAY = 0x33,            // нет фидбека тэна
        [Description("Heater enabled without water")]
        TRY_SET_HEAT_WITHOUT_WATER = 0x34,  // включение нагрева без воды
        [Description("Set temperature over 90")]
        SET_HEAT_OVER90 = 0x35,             // попытка выставить температуру выше 90

        #endregion

        #region Engine

        [Description("Engine breakdown")]
        NO_ENGINE = 0x40,                   // нет фидбека двигателя
        [Description("Engine CW relay sticking")]
        ENGINE_RELAYCW_STICKING = 0x41,     // залипание реле двигателя 1
        [Description("Engine CCW relay sticking")]
        ENGINE_RELAYCCW_STICKING = 0x42,    // залипание реле двигателя 2
        [Description("Tachometer error")]
        NO_TACHO = 0x43,                    // нет сигнала с тахометра или двигатель не раскручивается

        #endregion

        #region Door

        [Description("Locker fault")]
        NO_LOCKER = 0x50,                   // нет сигнала от защелки
        [Description("Locker triak break")]
        BAD_DOOR_TRIAK = 0x51,              // нет фидбэка от защелки
        [Description("Locker triak breakdown")]
        BAD_DOOR_TRIAK2 = 0x52,             // есть фидбэк от защелки при отключенном симисторе
        [Description("Unlock with water")]
        TRY_OPEN_DOOR_WITH_WATER = 0x53,    // попытка разблокировать дверцу при набранной воде

        #endregion

        #region Valve

        [Description("Open water with open door")]
        TRY_OPEN_WATER_WITH_OPEN_DOOR = 0x60,   // попытка набрать воду при открытой дверце

        #endregion

        #region EEPROM

        //eeprom
        [Description("EEPROM empty")]
        EEPROM_EMPTY = 0x70,                // нет маркера записи
        [Description("EEPROM bad CRC")]
        EEPROM_BADCRC = 0x71,               // неправильный crc
        [Description("EEPROM write verify error")]
        EEPROM_LOCK = 0x72,                 // записанные данные не совпадают с записываемыми
        [Description("EEPROM write error")]
        EEPROM_WRITEERROR = 0x73,			// не удалось записать флешкy

        #endregion
    }
}
