import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'dart:async';
import 'dart:typed_data';

class OpenWasherApi {
  BluetoothConnection connection;

  OpenWasherApi(String address) {
    BluetoothConnection.toAddress(address).then((_connection) {
      print('Connected to the device');
      connection = _connection;

      connection.input.listen(_onDataReceived).onDone(() {});
    }).catchError((error) {
      print("Connect failed: " + error);
    });
  }

  void dispose() {
    connection.dispose();
  }

  void _onDataReceived(Uint8List data) {}

  Future ping() async {
    await sendCommand(new Uint8List(0));
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

  int getCrc(Uint8List data, int count) {
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
}
