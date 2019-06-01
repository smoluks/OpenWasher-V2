#include "stm32f10x.h"
#include "main.h"
#include "events.h"
#include "commands.h"
#include "status.h"
#include <string.h>
#include <stdbool.h>
#include <uart_driver.h>

extern volatile bool ct;

void processcommand(uint8_t* buffer, uint8_t count)
{
	switch(buffer[0])
	{
		case 0x01:
			if(count-- < 1){
				send_answer(0x01, BADARGS);
				return;
			}

			if(count > sizeof(actionargs))
				count = sizeof(actionargs);

			memset((uint8_t*)actionargs, 0xFF, sizeof(actionargs));
			memcpy((uint8_t*)actionargs, buffer+1, count);

			action |= START_PROGRAM;
			send_answer(0x01, NOEEROR);
			break;
		case 0x02:
			ct = true;
			send_answer(0x02, NOEEROR);
			break;
		case 0x03:
			send_answer_with_data(0x03, get_status(), sizeof(struct status_s));
			break;
		case 0x0A:
			send_answer(0x0A, NOEEROR);
			action |= GOTOBOOTLOADER;
			break;
		default:
			send_answer(buffer[0], UNSUPPORTEDCOMMAND);
			break;
	}
}
