#include <crosszero_driver.h>
#include <door_driver.h>
#include <engine_driver.h>
#include <engine_hardware.h>
#include <error.h>
#include <pump_driver.h>
#include <stdint-gcc.h>
#include <stm32f10x.h>
#include <systick.h>
#include <therm_driver.h>
#include "crosszero_driver.h"

volatile uint16_t voltage;
extern volatile uint8_t tacho_currentrps;

void ADC1_2_IRQHandler(void) {
	if (ADC1->SR & ADC_SR_JEOC) {
		processRawTemperature(ADC1->JDR1);
		ADC1->SR &= ~ADC_SR_JEOC;
	}
	if (ADC2->SR & ADC_SR_JEOC) {
		voltage = ADC2->JDR1;
		process_tacho(ADC2->JDR2);

		ADC2->SR &= ~ADC_SR_JEOC;
	}
}

//фидбэк двигателя
volatile uint32_t engine_feedback_timestamp = 0;
void EXTI0_IRQHandler(void) {
	EXTI->PR |= 0x0001;
	EXTI->PR;

	//замеряем длительность положительного импульса
	if (GPIOB->IDR & 0x0001) {
		//фронт
		engine_feedback_timestamp = get_systime();
	} else {
		//спад
		uint32_t dt = delta(engine_feedback_timestamp);
		if (dt >= 1 && dt <= 2)
			process_feedback(ef_full);
		else if(dt <= 11)
			process_feedback(ef_regul);
		else if (dt <= 13)
			process_feedback(ef_half);
	}
}

//переполнение
void EXTI1_IRQHandler(void) {
	EXTI->PR |= 0x0002;
	EXTI->PR;

	set_error(OVERFLOW);
}

//фидбэк тэна
extern volatile bool thermfeedbackispresent;
extern uint32_t thermfeedback_timestamp;

void EXTI3_IRQHandler(void) {

	EXTI->PR |= 0x0008;
	EXTI->PR;

	thermfeedbackispresent = true;
	thermfeedback_timestamp = get_systime();
}

//cross-zero
void EXTI4_IRQHandler(void) {

	EXTI->PR |= 0x0010;
	crosszero_irq(GPIOA->IDR & 0x0010);
}

//pump feedback
volatile uint32_t lastpumptime = 0;
volatile int32_t pump_raw = -1;
void EXTI15_10_IRQHandler(void) {

	EXTI->PR |= 0x1000;
	EXTI->PR;

	pump_raw = delta(lastpumptime);
	lastpumptime = get_systime();

	if (pump_raw > 8 && pump_raw < 12) {
		pump_feedback();
	}
}

//clear tacho_currentrps
void TIM3_IRQHandler(void) {
	TIM3->SR &= ~TIM_SR_UIF;

	tacho_currentrps = 0;

	while (TIM3->SR & TIM_SR_UIF);
}

//engine triak on
void TIM4_IRQHandler(void) {
	TIM4->SR &= ~TIM_SR_UIF;

	engine_triakon();
	TIM2->CR1 &= ~TIM_CR1_CEN;
	TIM2->CNT = 0;
	TIM2->CR1 |= TIM_CR1_CEN; //запуск таймера задержки

	while (TIM4->SR & TIM_SR_UIF);
}

//engine triak off
void TIM2_IRQHandler(void) {
	TIM2->SR &= ~TIM_SR_UIF;

	engine_triakoff();

	while (TIM2->SR & TIM_SR_UIF);
}
