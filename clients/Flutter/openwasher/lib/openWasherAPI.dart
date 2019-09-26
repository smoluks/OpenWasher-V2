import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'dart:async';
import 'dart:typed_data';

class Status {
  int program;
  int stage;
  int temperature;
  int timefull;
  int timepassed;

  Status(Uint8List data) {
    if (data.length != 11) throw Exception("bad status packet length");

    program = data[0];
    stage = data[1];
    temperature = data[2];
    timefull = data[6] << 24 + data[5] << 16 + data[4] << 8 + data[3];
    timepassed = data[10] << 24 + data[9] << 16 + data[8] << 8 + data[7];
  }
}

class OpenWasherApi {
  BluetoothConnection connection;

  Completer pingCompleter;
  Completer startProgramCompleter;
  Completer breakProgramCompleter;
  Completer<Status> getStatusCompleter;

  OpenWasherApi(String address) {
    BluetoothConnection.toAddress(address).then((_connection) {
      print('Connected to the device');
      connection = _connection;

      connection.input.listen(_onDataReceived).onDone(() {});
    });
  }

  void dispose() {
    connection?.dispose();
  }

  bool isConnected() {
    return connection == null;
  }

  void _onDataReceived(Uint8List data) {
    if (data.length < 3) {
      print("Loo small packet length: " + data.length.toString());
      return;
    }
    if (data[0] != 0xAB) {
      print("bad start marker: 0x" + data[0].toRadixString(16));
      return;
    }
    if (data[1] != (data.length + 3)) {
      print("bad length, expected: " +
          (data[1] + 3).toString() +
          ", real: " +
          data.length.toString());
      return;
    }
    if (getCrc(data) != 0) {
      print("Bad CRC");
      return;
    }

    if (data.length == 3) {
      pingCompleter?.complete();
    } else {
      switch (data[2]) {
        case 1:
          startProgramCompleter?.complete();
          break;
        case 2:
          breakProgramCompleter?.complete();
          break;
        case 3:
          getStatusCompleter
              ?.complete(new Status(data.sublist(3, data.length - 2)));
          break;
        case 0x5E:
          processEvent(data[3], data.sublist(4, data.length - 2));
          break;
        case 0x5B:
          processError(data[3], data.sublist(4, data.length - 2));
          break;
        case 0x58:
          processMessage(new String.fromCharCodes(data, 3, data.length - 2));
          break;
      }
    }
  }

  Future ping() async {
    if (pingCompleter != null && !pingCompleter.isCompleted) {
      throw Exception("ping: call from some threads");
    }

    pingCompleter = new Completer();

    await sendCommand(new Uint8List(0));

    await pingCompleter.future.timeout(Duration(seconds: 15));
  }

  Future startProgram(int program,
      [int temperature = 0xFF,
      int duration = 0xFF,
      int washingspeed = 0xFF,
      int spinningspeed = 0xFF,
      int waterlevel = 0xFF,
      int rinsingCycles = 0xFF]) async {
    if (startProgramCompleter != null && !startProgramCompleter.isCompleted) {
      throw Exception("startProgram: call from some threads");
    }

    startProgramCompleter = new Completer();
    var data = new Uint8List(8);
    data[0] = 1;
    data[1] = program;
    data[2] = temperature;
    data[3] = duration;
    data[4] = washingspeed;
    data[5] = spinningspeed;
    data[6] = waterlevel;
    data[7] = rinsingCycles;
    await sendCommand(data);

    await startProgramCompleter.future.timeout(Duration(seconds: 15));
  }

  Future stopProgram() async {
    if (breakProgramCompleter != null && !breakProgramCompleter.isCompleted) {
      throw Exception("stopProgram: call from some threads");
    }

    breakProgramCompleter = new Completer();
    var data = new Uint8List(1);
    data[0] = 2;
    await sendCommand(data);

    await breakProgramCompleter.future.timeout(Duration(seconds: 15));
  }

  Future<Status> getStatus() async {
    if (getStatusCompleter != null && !getStatusCompleter.isCompleted) {
      throw Exception("getStatus: call from some threads");
    }

    getStatusCompleter = new Completer();
    var data = new Uint8List(1);
    data[0] = 3;
    await sendCommand(data);

    return await getStatusCompleter.future.timeout(Duration(seconds: 15));
  }

  Future sendCommand(Uint8List command) async {
    Uint8List data = new Uint8List(command.length + 3);
    data[0] = 0xAB;
    data[1] = command.length;
    if (command.length > 0) data.setRange(2, command.length + 2, command);
    data[command.length + 2] = getCrc(data, command.length + 2);

    connection.output.add(data);
    await connection.output.allSent;
  }

  int getCrc(Uint8List data, [int count = -1]) {
    if (count == -1) count = data.length;

    int crc = 0;

    for (int index = 0; index < count; index++) {
      var currentByte = data[index];

      for (int bitCounter = 0; bitCounter < 8; bitCounter++) {
        if (((crc ^ currentByte) & 0x01) != 0) {
          crc = ((crc >> 1) ^ 0x8C);
        } else {
          crc >>= 1;
        }
        currentByte >>= 1;
      }
    }

    return crc;
  }

  void processEvent(int data, Uint8List sublist) {}

  void processError(int data, Uint8List sublist) {}

  void processMessage(String string) {}
}
