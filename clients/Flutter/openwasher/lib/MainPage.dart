import 'dart:async';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'BluetoothDiscoveryPage.dart';
import 'Dto/OpenWasherDevice.dart';
import 'SelectProgramPage.dart';

class MainPage extends StatefulWidget {
  @override
  _MainPage createState() => new _MainPage();
}

class _MainPage extends State<MainPage> {
  @override
  void initState() {
    super.initState();

    enableBluetooth();
  }

  Future<void> enableBluetooth() async {
    var state = await FlutterBluetoothSerial.instance.state;

    if (!state.isEnabled) {
      //
      await FlutterBluetoothSerial.instance.requestEnable();
      //
      FlutterBluetoothSerial.instance
          .onStateChanged()
          .listen((BluetoothState state) {
        if (state.isEnabled)
          connectToLastDevice();
        else
          Navigator.of(context).pop();
      });

      return;
    }

    connectToLastDevice();
  }

  Future<void> connectToLastDevice() async {
    final prefs = await SharedPreferences.getInstance();

    OpenWasherDevice device;

    var address = prefs.getString("address") ?? null;
    if (address != null) {
      var name = prefs.getString("name") ?? null;
      device = new OpenWasherDevice(address, name);
    } else {
      device = await Navigator.of(context)
          .push(MaterialPageRoute(builder: (context) {
        return BluetoothDiscoveryPage();
      }));
      if (device == null) return;

      prefs.setString("address", device.address);
      prefs.setString("name", device.name);
    }

    bool success =
        await Navigator.of(context).push(MaterialPageRoute(builder: (context) {
      return SelectProgramPage(device: device);
    }));

    if (!success) {
      prefs.setString("address", null);
    }
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: Scaffold(
        body: Center(
            child: new CupertinoActivityIndicator(
          radius: 50,
        )),
      ),
    );
  }
}
