#pragma once

//clock regs
#define  RTCSEC          0x00
#define  RTCSEC_ST       1 << 7
#define  RTCMIN          0x01
#define  RTCHOUR         0x02
#define  RTCWKDAY          0x03
#define  RTCWKDAY_OSCRUN   1 << 5
#define  RTCWKDAY_PWRFAIL  1 << 4
#define  RTCWKDAY_VBATEN   1 << 3
#define  RTCDATE         0x04
#define  RTCMTH         0x05
#define  RTCYEAR         0x06
#define  CONTROL         0x07
#define  OSCTRIM          0x08
#define  EEUNLOCK         0x09
#define  ALM0SEC      0x0a
#define  ALM0MIN      0x0b
#define  ALM0HOUR       0x0c
#define  ALM0WKDAY     0x0d
#define  ALM0DATE     0x0e
#define  ALM0MTH      0x0f
#define  ALM1SEC      0x11
#define  ALM1MIN      0x12
#define  ALM1HOUR       0x13
#define  ALM1WKDAY     0x14
#define  ALM1DATE     0x15
#define  ALM1MTH      0x16
#define  PWRDNMIN     0x18
#define  PWRDNHOUR    0x19
#define  PWRDNDATE    0x1a
#define  PWRDNMTH     0x1b
#define  PWRUPMIN     0x1c
#define  PWRUPHOUR    0x1d
#define  PWRUPDATE    0x1e
#define  PWRUPMTH     0x1f

void initClock();
