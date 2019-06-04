#include <stdio.h>
#include <stdbool.h>
#include "therm_driver.h"
#include "therm_hardware.h"
#include "valve_hardware.h"
#include "systick.h"
#include "eeprom.h"
#include "delay.h"

volatile uint16_t rawtemperature;
volatile uint16_t targettemperature = 0;
volatile bool thermfeedbackispresent = false;
volatile bool therm_manualcontrol = false;

extern volatile bool ct;

bool therm_test() {
	printf("Test temperature sensor\n");

	if (thermfeedbackispresent) {
		printf("Test temperature need off heat\n");
		return false;
	}

	if (!is_water()) {
		printf("Test temperature need water\n");
		return false;
	}

	//check values
	printf("Water t %u\n", get_temperature());
	if (get_temperature() < 10) {
		set_error(NTC_NOT_PRESENT);
		return false;
	}

	//check noise
	delay_ms_with_ct(60000u); //остывание датчика
	if (ct)
		return false;

	uint16_t tmax = 0;
	uint16_t tmin = 0xFFFF;
	uint32_t timestamp = get_systime();
	while (checkdelay(timestamp, 60000u)) {
		if (tmax < rawtemperature)
			tmax = rawtemperature;

		if (tmin > rawtemperature)
			tmin = rawtemperature;
	}
	if (ct)
		return false;

	eeprom_config.temperaturenoise = tmax - tmin;
	printf("Rmax %u, Rmin %u\n", tmax, tmin);

	//check relay on
	therm_manualcontrol = true;
	heater_enable();

	timestamp = get_systime();
	while (!thermfeedbackispresent && checkdelay(timestamp, 3000u));
	if (ct) {
		heater_disable();
		therm_manualcontrol = false;
		return false;
	}
	if (!thermfeedbackispresent) {
		set_error(BAD_HEATER_RELAY);
		return false;
	}
	printf("Relay feedback at %lu\n", delta(timestamp));

	//check temperature risen
	timestamp = get_systime();
	while (rawtemperature < tmax + 42 && checkdelay(timestamp, 150000u));
	if (ct) {
		heater_disable();
		therm_manualcontrol = false;
		return false;
	}
	if (rawtemperature < tmax + 42) {
		set_error(NO_HEATER);
		return false;
	}
	printf("Heat at %lu\n", delta(timestamp));

	//check relay disable
	heater_disable();
	timestamp = get_systime();
	while (thermfeedbackispresent && checkdelay(timestamp, 3000u));
	if (thermfeedbackispresent) {
		set_error(HEATER_RELAY_STICKING);
		return false;
	}

	therm_manualcontrol = false;
	printf("Relay off feedback at %lu\nTest heater OK\n", delta(timestamp));

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

inline uint8_t get_temperature() {
	return rawtemperature * 4 / 169 + 8;
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
	else if (rawtemperature < (targettemperature - THERM_HYSTERESIS * 42 - eeprom_config.temperaturenoise))
		heater_enable();
}

uint32_t thermfeedback_timestamp = 0;
inline void therm_systick() {
	if (thermfeedbackispresent && !checkdelay(thermfeedback_timestamp, 13))
		thermfeedbackispresent = false;
}

inline void therm_adc(uint16_t value) {
	rawtemperature = value;
	if (value > 3600)
		set_error(OVERHEAT);
}

inline void therm_emergencydisable() {
	therm_manualcontrol = true;
	heater_disable();
	targettemperature = 0;
}
