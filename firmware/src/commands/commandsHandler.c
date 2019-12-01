#include <stdint.h>
#include "commandsRoutine.h"

#include "configCommand.h"
#include "defaultOptionsCommand.h"
#include "goToBootloaderModeCommand.h"
#include "startProgramCommand.h"
#include "statusCommand.h"
#include "stopProgramCommand.h"
#include "timeCommand.h"

void processCommand(uint8_t* buffer, uint8_t count) {
	if(!count)
		send_empty_answer();
	else switch (buffer[0]) {
	case startProgramPacketType:
		processStartProgramCommand(buffer, count);
		break;
	case stopProgramPacketType:
		processStopProgramCommand(buffer, count);
		break;
	case getProgramDefaultsPacketType:
		processDefaultOptionsCommand(buffer, count);
		break;
	case goToBootloaderModePacketType:
		processGoToBootloaderModeCommand(buffer, count);
		break;
	case getStatusPacketType:
		processStatusCommand(buffer, count);
		break;
	case setSetConfigPacketType:
		processConfigCommand(buffer, count);
		break;
	case getSetTimePacketType:
		processTimeCommand(buffer, count);
		break;
	default:
		send_answer(buffer[0], UNSUPPORTEDCOMMAND);
		break;
	}
}


