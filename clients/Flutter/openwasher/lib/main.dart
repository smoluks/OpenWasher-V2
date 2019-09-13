import 'package:flutter/material.dart';
import 'devicesList.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'OpenWasher client',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: DevicesList(),
    );
  }
}
