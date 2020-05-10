import 'dart:async';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'BluetoothDiscoveryPage.dart';
import 'Dto/OpenWasherDevice.dart';
import 'Dto/ReceivedNotification.dart';
import 'Helpers/Dialogs.dart';
import 'SecondScreen.dart';
import 'SelectProgramPage.dart';
import 'main.dart';

class MainPage extends StatefulWidget {
  @override
  _MainPage createState() => new _MainPage();
}

class _MainPage extends State<MainPage> {
  bool _loading = true;

  @override
  void initState() {
    super.initState();

    _requestIOSPermissions();
    _configureDidReceiveLocalNotificationSubject();
    _configureSelectNotificationSubject();

    enableBluetooth();
  }

  @override
  void dispose() {
    didReceiveLocalNotificationSubject.close();
    selectNotificationSubject.close();
    super.dispose();
  }

  Future<void> enableBluetooth() async {
    var state = await FlutterBluetoothSerial.instance.state;

    if (state.isEnabled) connectToLastDevice();

    await FlutterBluetoothSerial.instance.requestEnable();

    state = await FlutterBluetoothSerial.instance.state;
    if (state.isEnabled) connectToLastDevice();

    Dialogs.showErrorBox(context, "Can't enable bluetooth. Enable it manually",
        () {
      Navigator.of(context).pop();
    });
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

    if (success != true) {
      prefs.setString("address", null);
    }

    _loading = false;
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: Scaffold(
        body: _loading
            ? Center(
                child: new CupertinoActivityIndicator(
                radius: 50,
              ))
            : Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                    RaisedButton(
                      shape: new RoundedRectangleBorder(
                          borderRadius: new BorderRadius.circular(18.0),
                          side: BorderSide(color: Colors.blue)),
                      color: Colors.blue,
                      textColor: Colors.white,
                      padding: EdgeInsets.all(8.0),
                      onPressed: () {
                        connectToLastDevice();
                      },
                      child: Text(
                        "Connect".toUpperCase(),
                        style: TextStyle(
                          fontSize: 20.0,
                        ),
                      ),
                    )
                  ]),
      ),
    );
  }

  void _requestIOSPermissions() {
    flutterLocalNotificationsPlugin
        .resolvePlatformSpecificImplementation<
            IOSFlutterLocalNotificationsPlugin>()
        ?.requestPermissions(
          alert: true,
          badge: true,
          sound: true,
        );
  }

  void _configureDidReceiveLocalNotificationSubject() {
    didReceiveLocalNotificationSubject.stream
        .listen((ReceivedNotification receivedNotification) async {
      await showDialog(
        context: context,
        builder: (BuildContext context) => CupertinoAlertDialog(
          title: receivedNotification.title != null
              ? Text(receivedNotification.title)
              : null,
          content: receivedNotification.body != null
              ? Text(receivedNotification.body)
              : null,
          actions: [
            CupertinoDialogAction(
              isDefaultAction: true,
              child: Text('Ok'),
              onPressed: () async {
                Navigator.of(context, rootNavigator: true).pop();
                await Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) =>
                        SecondScreen(receivedNotification.payload),
                  ),
                );
              },
            )
          ],
        ),
      );
    });
  }

  void _configureSelectNotificationSubject() {
    selectNotificationSubject.stream.listen((String payload) async {
      // await Navigator.push(
      //   context,
      //   MaterialPageRoute(builder: (context) => SecondScreen(payload)),
      // );
    });
  }
}
