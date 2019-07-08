#include <system_stm32f10x.h>
#include "stm32f10x.h"
#include "delay.h"
#include "error.h"

void __attribute__ ((weak)) _init(void) {
}

void SystemInit(void) {
	initError = SystemCoreClockUpdate();

	if (initError) {
		//only uart
		RCC->APB2ENR = RCC_APB2ENR_IOPAEN | RCC_APB2ENR_USART1EN
				| RCC_APB2ENR_AFIOEN;
		GPIOA->CRH = 0x088008B0;

		USART1->BRR = 69;
		USART1->CR2 = 0x00000000;
		USART1->CR3 = 0x00000000;
		USART1->CR1 = USART_CR1_UE | USART_CR1_TE | USART_CR1_RE
				| USART_CR1_RXNEIE | USART_CR1_PCE | USART_CR1_M;
		return;
	}

	RCC->APB1ENR = RCC_APB1ENR_I2C2EN | RCC_APB1ENR_PWREN | RCC_APB1ENR_TIM2EN | RCC_APB1ENR_TIM3EN
			| RCC_APB1ENR_TIM4EN;
	RCC->APB2ENR = RCC_APB2ENR_ADC1EN | RCC_APB2ENR_IOPAEN | RCC_APB2ENR_IOPBEN
			| RCC_APB2ENR_IOPCEN | RCC_APB2ENR_USART1EN | RCC_APB2ENR_AFIOEN
			| RCC_APB2ENR_ADC2EN;
	AFIO->MAPR = AFIO_MAPR_SWJ_CFG_1;
	//porta
	//A0 - NTC
	//A1 - датчик переполнения (подтяжка вниз)
	//A2 - датчик наполнения (аналог)
	//A3 - фидбэк тена (подтяжка вверх)
	//A4 - ZC (подтяжка вниз)
	//A5 - напряжение питания
	//A6 - power_en(+)
	//A7 - симистор двигателя
	//A8 - блокировка люка
	//A9 - TX
	//A10 - RX
	//A11 - реле насоса
	//A12 - реле двигателя 1
	//A13 - SWDIO
	//A14 - SWCLK
	//A15 -
	GPIOA->CRH = 0x088228B2;
	GPIOA->CRL = 0x32088080;
	GPIOA->ODR = 0x00000448;
	//portb
	//B0 - фидбэк двигателя (подтяжка вверх)
	//B1 - тахометр (аналог)
	//B2 - Датчик наполнения N1 (подтяжка вниз)
	//B3 -
	//B4 - фидбэк тена через резистор
	//B5 -
	//B6 - реле двигателя 2
	//B7 - реле тена
	//B8 - BT MODE
	//B9 - BT RESET
	//B10 - SCL
	//B11 - SDA
	//B12 - фидбэк насоса (подтяжка вверх)
	//B13 - клапан2
	//B14 - симистор насоса
	//B15 - клапан1
	GPIOB->CRH = 0x3338FF62;
	GPIOB->CRL = 0x22020808;
	GPIOB->ODR = 0x00001011;
	//portc
	//C13 - LED green
	//C14 - LED orange
	GPIOC->CRH = 0x02200000;
	GPIOC->CRL = 0x00000000;
	GPIOC->ODR = 0x00004000;
	//UART1
	USART1->BRR = 0x271;
	USART1->CR2 = 0x00000000;
	USART1->CR3 = 0x00000000;
	USART1->CR1 = USART_CR1_UE | USART_CR1_TE | USART_CR1_RE | USART_CR1_PCE
			| USART_CR1_M | USART_CR1_RXNEIE | USART_CR1_IDLEIE;
	NVIC_SetPriority(USART1_IRQn, 3);
	NVIC_EnableIRQ(USART1_IRQn);
	//ADC1
	//JDR1 - 0 - t
	ADC1->SMPR2 = (7 << ADC_SMPR2_SMP0_POS);
	ADC1->CR2 = ADC_CR2_JEXTSEL;
	ADC1->CR2 |= ADC_CR2_JEXTTRIG;
	ADC1->CR2 |= ADC_CR2_CONT;
	ADC1->CR1 |= ADC_CR1_JAUTO | ADC_CR1_SCAN | ADC_CR1_JEOCIE;
	ADC1->JSQR = (0 << ADC_JSQR_JSQ4_POS);
	ADC1->CR2 |= ADC_CR2_ADON;
	//-----ADC2-----
	//JDR1 - 5 - voltage
	//JDR2 - 9 - tacho
	ADC2->SMPR2 = (7 << ADC_SMPR2_SMP9_POS) | (7 << ADC_SMPR2_SMP5_POS);
	ADC2->CR2 = ADC_CR2_JEXTSEL;
	ADC2->CR2 |= ADC_CR2_JEXTTRIG;
	ADC2->CR2 |= ADC_CR2_CONT;
	ADC2->CR1 |= ADC_CR1_JAUTO | ADC_CR1_SCAN | ADC_CR1_JEOCIE;
	ADC2->JSQR = (1 << ADC_JSQR_JL_POS) | (5 << ADC_JSQR_JSQ3_POS)
			| (9 << ADC_JSQR_JSQ4_POS);
	ADC2->CR2 |= ADC_CR2_ADON;
	//прогрев АЦП
	delay_us(1);
	//Запуск калибровки обоих АЦП
	ADC2->CR2 |= ADC_CR2_CAL;
	ADC1->CR2 |= ADC_CR2_CAL;
	while ((ADC1->CR2 & ADC_CR2_CAL) || (ADC2->CR2 & ADC_CR2_CAL))
		;
	//
	delay_us(10);
	ADC1->CR2 |= ADC_CR2_JSWSTART;
	ADC2->CR2 |= ADC_CR2_JSWSTART;
	NVIC_SetPriority(ADC1_2_IRQn, 2);
	NVIC_EnableIRQ(ADC1_2_IRQn);
	//EINT
	//B0 - фидбэк двигателя
	//A1 - OVF
	//A3 - фидбэк тена
	//A4 - cross-zero
	//B12 - pump feedback
	AFIO->EXTICR[3] = 0x0001;
	AFIO->EXTICR[2] = 0x0000;
	AFIO->EXTICR[1] = 0x0000;
	AFIO->EXTICR[0] = 0x0001;
	EXTI->IMR = 0x0000101B;
	EXTI->RTSR = 0x0000101B;
	EXTI->FTSR = 0x00000019;
	EXTI->PR = 0xFFFF;
	NVIC_EnableIRQ(EXTI0_IRQn);
	//NVIC_EnableIRQ(EXTI1_IRQn);
	NVIC_EnableIRQ(EXTI3_IRQn);
	NVIC_EnableIRQ(EXTI4_IRQn);
	NVIC_EnableIRQ(EXTI15_10_IRQn);
	//----TIM2 - engine triak off-----
	TIM2->PSC = 72 - 1;
	TIM2->CR1 = TIM_CR1_OPM;
	TIM2->ARR = 100;
	TIM2->EGR |= TIM_EGR_UG;
	delay_us(1);
	TIM2->SR &= ~(TIM_SR_UIF);
	TIM2->DIER = TIM_DIER_UIE;
	NVIC_SetPriority(TIM2_IRQn, 1);
	NVIC_EnableIRQ(TIM2_IRQn);
	//----TIM3 - tacho - 100KHz-----
	TIM3->PSC = 720 - 1;
	TIM3->CR1 = TIM_CR1_OPM;
	TIM3->ARR = 18750;
	TIM3->EGR |= TIM_EGR_UG;
	delay_us(1);
	TIM3->SR &= ~(TIM_SR_UIF);
	TIM3->DIER = TIM_DIER_UIE;
	NVIC_SetPriority(TIM3_IRQn, 1);
	NVIC_EnableIRQ(TIM3_IRQn);
	//----TIM4 - управление двигателем - 3,6MHz-----
	TIM4->PSC = 20 - 1;
	TIM4->CR1 = TIM_CR1_OPM;
	TIM4->ARR = 36000;
	TIM4->EGR |= TIM_EGR_UG;
	delay_us(1);
	TIM4->SR &= ~(TIM_SR_UIF);
	TIM4->DIER = TIM_DIER_UIE;
	NVIC_SetPriority(TIM4_IRQn, 1);
	NVIC_EnableIRQ(TIM4_IRQn);
	//-----I2C-----
	I2C2->CCR = 179;
	I2C2->TRISE = 72;
	I2C2->CR2 |= 36;
	I2C2->CR1 |= I2C_CR1_PE;
	//systick
	SysTick->LOAD = 72000000UL / 1000 - 1;
	SysTick->VAL = 72000000UL / 1000 - 1;
	SysTick->CTRL = SysTick_CTRL_CLKSOURCE_Msk | SysTick_CTRL_TICKINT_Msk
			| SysTick_CTRL_ENABLE_Msk;
	//iwdg
	// Для IWDG_PR=7 Tmin=6,4мс RLR=Tмс*40/256
	//IWDG->KR=0x5555; // Ключ для доступа к таймеру
	//IWDG->PR=7; // Обновление IWDG_PR
	//IWDG->RLR=1000*40/256; // Загрузить регистр перезагрузки
	//IWDG->KR=0xAAAA; // Перезагрузка
	//IWDG->KR=0xCCCC; // Пуск таймера
	//
	__enable_irq();
}

enum errorcode SystemCoreClockUpdate() {
	//включаем внешний кварц
	RCC->CR = RCC_CR_HSEON;
	volatile uint16_t timeout = 0;
	while (!(RCC->CR & RCC_CR_HSERDY) && timeout++ != 5000) //ждем готовность внешнего кварца
	{
	}
	if (!(RCC->CR & RCC_CR_HSERDY))
		return CRYSTAL;

	FLASH->ACR = FLASH_ACR_PRFTBE | FLASH_ACR_LATENCY_1; //делитель частоты для FLASH памяти
	RCC->CFGR = RCC_CFGR_PLLMULL9 | RCC_CFGR_PLLSRC | RCC_CFGR_ADCPRE_DIV8
			| RCC_CFGR_PPRE2_DIV1 | RCC_CFGR_PPRE1_DIV2 | RCC_CFGR_HPRE_DIV1;

	RCC->CR |= RCC_CR_PLLON; //стартуем PLL
	timeout = 0;
	while (!(RCC->CR & RCC_CR_PLLRDY) && timeout++ != 5000) //ждем готовности PLL
	{
	}
	if (!(RCC->CR & RCC_CR_PLLRDY))
		return PLL;

	RCC->CFGR = RCC->CFGR | RCC_CFGR_SW_PLL; //переключаемся на pll
	timeout = 0;
	while ((RCC->CFGR & RCC_CFGR_SWS) != RCC_CFGR_SWS_PLL && timeout++ != 5000) //ждем переключения на pll
	{
	}
	if ((RCC->CFGR & RCC_CFGR_SWS) != RCC_CFGR_SWS_PLL)
		return SW_PLL;

	RCC->CSR |= RCC_CSR_LSION;
	timeout = 0;
	while (!(RCC->CSR & RCC_CSR_LSIRDY) && timeout++ != 5000)
		;
	if (!(RCC->CSR & RCC_CSR_LSIRDY))
		return LSI;

	return NOERROR;
}

