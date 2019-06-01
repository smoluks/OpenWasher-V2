/*
 * led_driver.c
 *
 *  Created on: Feb 5, 2019
 *      Author: Shironeko
 */

#include "stm32f10x.h"
#include "led_hardware.h"

enum state {
	led_state_off = 0, led_state_on = 1, led_state_blink = 2
};

enum state orangeled_state = led_state_on;
uint16_t orangeled_off_time;
uint16_t orangeled_on_time;
uint32_t orangetimestamp;
void set_orangeled_blink(uint16_t offtime, uint16_t ontime) {
	if (!ontime) {
		orangeled_state = led_state_off;
		led_orange_off();
	} else if (!offtime) {
		orangeled_state = led_state_on;
		led_orange_on();
	} else {
		orangeled_state = led_state_blink;
		orangeled_off_time = offtime;
		orangeled_on_time = ontime;
		orangetimestamp = get_systime();
	}
}

enum state greenled_state = led_state_off;
uint16_t greenled_off_time;
uint16_t greenled_on_time;
uint32_t greentimestamp;
void set_greenled_blink(uint16_t offtime, uint16_t ontime) {
	if (!ontime) {
		greenled_state = led_state_off;
		led_green_off();
	} else if (!offtime) {
		greenled_state = led_state_on;
		led_green_on();
	} else {
		greenled_state = led_state_blink;
		greenled_off_time = offtime;
		greenled_on_time = ontime;
		greentimestamp = get_systime();
	}
}

void led_systick() {
	process_orange();
	process_green();
}

inline void process_orange() {
	if (orangeled_state != led_state_blink)
		return;

	if (check_time_passed(orangetimestamp)) {
		if (get_led_orange_state()) {
			//Off
			led_orange_off();
			orangetimestamp = get_systime() + orangeled_off_time;
		} else {
			led_orange_on();
			orangetimestamp = get_systime() + orangeled_on_time;
		}
	}
}

inline void process_green() {
	if (greenled_state != led_state_blink)
		return;

	if (check_time_passed(greentimestamp)) {
		if (get_led_green_state()) {
			//Off
			led_orange_off();
			greentimestamp = get_systime() + greenled_off_time;
		} else {
			led_orange_on();
			greentimestamp = get_systime() + greenled_on_time;
		}
	}
}

