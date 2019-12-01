#include <commandsRoutine.h>
#include <stdbool.h>
#include <stdio.h>
#include "bootloader.h"
#include "delay.h"
#include "eeprom.h"
#include "error.h"
#include "events.h"
#include "programs.h"
#include "programOptions.h"
#include "uart_driver.h"
#include "main.h"
#include "status.h"
#include "led_driver.h"
#include "engine_driver.h"
#include "hc05.h"
#include "programHandlers.h"
#include "watchdog.h"

extern volatile enum errorcode initError;
extern enum errorcode error;
extern program programFromCommand;
extern programOptions optionsFromCommand;
extern volatile bool ct;

volatile uint8_t action = 0;

void main(void)

{
	set_orangeled_blink(900, 100);

	send_event(POWER_ON);
	if(initError)
	{
		set_orangeled_blink(0, 100);
		set_greenled_blink(0, 100);
		send_error(initError);
		while(true) { WDT_RESET; }
	}

	initClock();
	readconfig();
	set_orangeled_blink(500, 500);

	hc05_init();
	set_orangeled_blink(100, 0);
	set_greenled_blink(0, 100);

	while (true) {
		WDT_RESET;

		if (error != NOERROR) {
			send_error(error);
			printf("Error %X\n", error);
			while (1) {
				if (action & GOTOBOOTLOADER) {
					send_event(GOTOBOOTLOADER);
					waittransmissionend();
					JumpToBootloader();
				}
			}
		}
		if (action & GOTOBOOTLOADER) {
			send_event(GOTOBOOTLOADER);
			waittransmissionend();
			JumpToBootloader();
		}
		if (action & START_PROGRAM) {

			send_event1args(START_PROGRAM, programFromCommand);
			status_set_program(programFromCommand, 1000000);
			ct = false;
			bool result = programs[programFromCommand](programFromCommand, optionsFromCommand);

			if (result)
				send_event1args(STOP_PROGRAM, programFromCommand);
			else
				send_event1args(BREAK_PROGRAM, programFromCommand);
			status_set_program(0xFF, 0);
			status_set_stage(STATUS_STOP);

		}
	}
}
