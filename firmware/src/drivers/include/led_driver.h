/*
 * led_driver.h
 *
 *  Created on: 3 θών. 2019 γ.
 *      Author: Shironeko
 */

#pragma once

void disableLedDriver();
void led_systick();
void set_orangeled_blink(uint16_t offtime, uint16_t ontime);
void set_greenled_blink(uint16_t offtime, uint16_t ontime);
