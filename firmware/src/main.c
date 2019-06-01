#include <bootloader.h>
#include <commands.h>
#include <delay.h>
#include <eeprom.h>
#include <error.h>
#include <events.h>
#include <programs.h>
#include <stdbool.h>
#include <stdint-gcc.h>
#include <uart_driver.h>
#include "main.h"
#include "status.h"
#include "led_hardware.h"
#include "engine_driver.h"
#include "hc05.h"

void process_program();

extern volatile enum errorcode initError;
extern volatile bool ct;

enum errorcode error = NOERROR;

void main(void)
{
	status_set_program(0xFF);
	set_orangeled_blink(900, 100);

	send_event(POWER_ON);
	if(initError)
	{
		set_orangeled_blink(0, 100);
		set_greenled_blink(0, 100);
		send_error(initError);
		while(true) { WDT_RESET; }
	}

	readconfig();
	set_orangeled_blink(500, 500);

	hc05_init();
	set_orangeled_blink(100, 0);
	set_greenled_blink(0, 100);

	while (true)
	{
		WDT_RESET;
		if(error != NOERROR)
		{
			send_error(error);
			error = NOERROR;
		}
		if (action & GOTOBOOTLOADER)
		{
			send_event(GOTOBOOTLOADER);
			waittransmissionend();
			JumpToBootloader();
		}
		if (action & START_PROGRAM)
		{
			process_program();
			action &= ~START_PROGRAM;
		}
	}
}

void process_program()
{

	uint8_t number = actionargs[0];
	if(number >= PROGRAM_COUNT)
		return;

	send_event1args(START_PROGRAM, number);
	status_set_program(number);

	ct = false;
	bool result = programs[number](number, (uint8_t*)actionargs+1);

	if(result)
		send_event1args(STOP_PROGRAM, number);
	else
		send_event1args(BREAK_PROGRAM, number);
	status_set_program(0xFF);
	status_set_stage(STATUS_STOP);
}
