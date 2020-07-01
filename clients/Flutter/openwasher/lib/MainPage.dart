import 'dart:async';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bluetooth_serial/flutter_bluetooth_serial.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:url_launcher/url_launcher.dart';

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
    if (state.isEnabled)
      connectToLastDevice();
    else
      Dialogs.showErrorBox(
          context, "Can't enable bluetooth. Enable it manually", () {
        Navigator.of(context).pop();
      });

    setState(() {
      _loading = false;
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
            : Padding(
                padding: EdgeInsets.all(30.0),
                child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: <Widget>[
                      Text(
                          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                          style: TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.bold,
                              color: Colors.black)),
                      GestureDetector(
                        onTap: _launchURL,
                        child: new Text(
                            "https://github.com/smoluks/OpenWasher-V2",
                            style: TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                                color: Colors.blue)),
                      ),
                      SizedBox(height: 30),
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
                    ])),
      ),
    );
  }

  _launchURL() async {
    const url = 'https://github.com/smoluks/OpenWasher-V2';
    if (await canLaunch(url)) {
      await launch(url);
    } else {
      throw 'Could not launch $url';
    }
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
