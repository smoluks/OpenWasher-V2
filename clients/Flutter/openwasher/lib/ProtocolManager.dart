import 'dart:async';

class ProtocolManager {
  Timer _pollingTimer;
  String _btAddress;

  var stateStream = new StreamController<WasherState>();

  ProtocolManager(this._btAddress) {
    _pollingTimer = new Timer.periodic(
        Duration(seconds: 10), (_) async => await updateState());
  }

  void dispose() {
    _pollingTimer?.cancel();
    stateStream.close();
  }

  updateState() {}

  getStateStream() {
    return stateStream.stream;
  }

  WasherState getState() {}
}

class WasherState {
  bool isRunning;
  Program program;
  Stage stage;
  int temperature;
  int speed;
  int programFullTime;
  int programPastTime;
}

enum Program { SelfTesting, Program1 }

enum Stage {
  Stop,
  LockDoor,
  UnlockDoor,
  Rinsing,
  Spinning,
  Prewashing,
  Washing,
  DrawWater,
  Sink,
  SelfTesting,
}
