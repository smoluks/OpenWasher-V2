/*
 * hc05.c
 *
 *  Created on: Feb 4, 2019
 *      Author: Shironeko
 */
#include <stdbool.h>
#include <string.h>
#include "hc05_uart_driver.h"

bool hc05_checkparms();

void hc05_init() {
	hc05_enter_setting_mode();

	hc05_checkparms();

	hc05_leave_setting_mode();
}

bool hc05_checkparms() {
	char* result = hc05_sendcommand("AT");
	if (strcmp(result, "OK"))
		return false;

	result = hc05_sendcommand("AT+NAME?");
	if (strcmp(result, "+NAME:WASH001 V2")) {
		result = hc05_sendcommand("AT+NAME=WASH001 V2");
		if (strcmp(result, "OK"))
			return false;
	}

	result = hc05_sendcommand("AT+PSWD?");
	if (strcmp(result, "+PSWD:1234")) {
		result = hc05_sendcommand("AT+PSWD=1234");
		if (strcmp(result, "OK"))
			return false;
	}

	result = hc05_sendcommand("AT+UART?");
	if (strcmp(result, "+UART:115200,0,2")) {
		result = hc05_sendcommand("AT+UART=115200,0,2");
		if (strcmp(result, "OK"))
			return false;
	}

	return true;
}

