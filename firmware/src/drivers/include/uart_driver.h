#pragma once
#include "stm32f10x.h"
#include "commands.h"
#include "events.h"

#define TXBUFFERSIZE 256
#define RXBUFFERSIZE 256

void send_answer(uint8_t command, enum commanderr_e state);
void send_answer_with_data(uint8_t command, uint8_t* data, uint8_t length);
void send_error(enum errorcode code);
void send_event(enum eventcode code);
void send_event1args(enum eventcode code, uint8_t arg);
void waittransmissionend();
