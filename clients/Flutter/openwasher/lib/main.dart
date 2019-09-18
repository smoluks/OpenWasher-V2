import 'package:flutter/material.dart';
import 'washerMain.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'OpenWasher client',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: WasherMain(),
    );
  }
}
