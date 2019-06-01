#pragma once

#define WDT_RESET IWDG->KR=0xAAAA
#define ACTION_GOTOBOOTLOADER 1 << 0

