#include <delay.h>
#include <error.h>
#include <eeprom.h>
#include <stdint-gcc.h>
#include <stdio.h>
#include <stm32f10x.h>
#include <systick.h>
#include <therm_driver.h>
#include <therm_hardware.h>

volatile uint16_t rawtemperature;
volatile uint16_t targettemperature = 0;
volatile bool thermfeedbackispresent = false;
volatile bool therm_manualcontrol = false;

extern volatile bool ct;

bool therm_test() {
	printf("Test temperature sensor\n");

	//check values
	printf("Water t %u\n", get_temperature());
	if (get_temperature() < 10) {
		set_error(NTC_NOT_PRESENT);
		return false;
	}

	if (!is_water()) {
		set_error(TRY_SET_HEAT_WITHOUT_WATER);
		return false;
	}

	//check noise
	delay_ms_with_ct(60000u); //остывание датчика
	if (ct)
		return false;

	uint16_t tmax = 0;
	uint16_t tmin = 0xFFFF;
	uint32_t timestamp = get_systime() + 60000U;
	while (!check_time_passed_with_ct(timestamp)) {
		if (tmax < rawtemperature)
			tmax = rawtemperature;

		if (tmin > rawtemperature)
			tmin = rawtemperature;
	}
	if (ct)
		return false;
	eeprom_config.temperaturenoise = tmax - tmin;
	printf("Rmax %u, Rmin %u\n", tmax, tmin);

	if (thermfeedbackispresent) {
		heater_disable();
		set_error(HEATER_RELAY_STICKING);
		return false;
	}
	therm_manualcontrol = true;
	heater_enable();

	//check relay on
	timestamp = get_systime() + 3000U;
	while (!thermfeedbackispresent && !check_time_passed_with_ct(timestamp));
	if (ct) {
		heater_disable();
		return false;
	}
	if (!thermfeedbackispresent) {
		heater_disable();
		set_error(BAD_HEATER_RELAY);
		return false;
	}
	printf("Relay feedback at %u\n", get_systime() - timestamp + 3000U);

	//check temperature rizen
	timestamp = get_systime() + 150000U;
	while (rawtemperature < tmax + 42
			&& !check_time_passed_with_ct(timestamp));
	if (ct) {
		heater_disable();
		return false;
	}
	if (check_time_passed_with_ct(timestamp)) {
		heater_disable();
		set_error(NO_HEATER);
		return false;
	}
	printf("Heat at %u\n", get_systime() - timestamp + 150000U);

	//check relay disable
	heater_disable();
	timestamp = get_systime() + 3000U;
	while (thermfeedbackispresent && !check_time_passed_with_ct(timestamp));
	if (thermfeedbackispresent) {
		set_error(HEATER_RELAY_STICKING);
		return false;
	}

	therm_manualcontrol = false;
	printf("Relay off feedback at %u\nSinking...\n", get_systime() - timestamp + 3000U);

	if(!sink(15000))
		return false;

	printf("Test heater OK\n");

	return true;
}

bool set_temperature(uint8_t t) {
	if (t < 20) {
		heater_disable();
		targettemperature = 0;
		return true;
	} else if (t > 90) {
		set_error(SET_HEAT_OVER90);
		return false;
	} else {
		targettemperature = ((uint16_t) t - 8) * 169 / 4;
		return true;
	}
}

inline void therm_emergencydisable() {
	heater_disable();
	targettemperature = 0;
}

inline void therm_crosszero() {
	if (therm_manualcontrol)
		return;

	if (targettemperature == 0) {
		heater_disable();
		return;
	}

	if (rawtemperature >= targettemperature)
		heater_disable();
	else if (rawtemperature < (targettemperature - (42 * 10) - eeprom_config.temperaturenoise))
		heater_enable();
}

uint32_t thermfeedback_timestamp = 0;

inline void therm_systick() {
	if (thermfeedbackispresent && !checkdelay(thermfeedback_timestamp, 15))
		thermfeedbackispresent = false;
}

inline void processRawTemperature(uint16_t temp) {
	rawtemperature = temp;
	if (temp > 3600) {
		heater_disable();
		targettemperature = 0;
		set_error(OVERHEAT);
	}
}

inline uint8_t get_temperature() {
	return rawtemperature * 4 / 169 + 8;
}
