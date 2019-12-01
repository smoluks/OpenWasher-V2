#pragma once

#include <commandsRoutine.h>
#include "stm32f10x.h"
#include "events.h"

#define TXBUFFERSIZE 256
#define RXBUFFERSIZE 256

void send(uint8_t data);
void send16(uint16_t data);
void send_event(enum eventcode code);
void send_event1args(enum eventcode code, uint8_t arg);
void send_error(enum errorcode code);
void waittransmissionend();
void waittransmissionend();
uint8_t addcrc(uint8_t crc, uint8_t data);
