import 'dart:async';
import 'package:flutter/material.dart';

import 'Dto/OpenWasherDevice.dart';
import 'ProtocolManager.dart';

class SelectProgramPage extends StatefulWidget {
  final OpenWasherDevice device;

  const SelectProgramPage({this.device});

  @override
  _SelectProgramPage createState() => new _SelectProgramPage();
}

class _SelectProgramPage extends State<SelectProgramPage> {
  ProtocolManager _protocolManager;
  bool _firstLoading = true;

  @override
  void initState() {
    super.initState();

    try {
      _protocolManager = new ProtocolManager(widget.device.address);
      var state = _protocolManager.getState();
      if(state == )
    } catch (ex) {
      Navigator.of(context).pop(-1);
    }
  }

  @override
  void dispose() {
    _protocolManager.dispose();

    super.dispose();
  }

  Future<void> updateState(WasherState state) async {
    setState(() {
      _state = state;
      _firstLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.device.name ?? '-'),
        actions: <Widget>[
          _firstLoading
              ? FittedBox(
                  child: Container(
                    margin: new EdgeInsets.all(16.0),
                    child: CircularProgressIndicator(
                      valueColor: AlwaysStoppedAnimation<Color>(
                        Colors.white,
                      ),
                    ),
                  ),
                )
              : Icon(Icons.play_arrow)
        ],
      ),
      body: Container(
        child: new ListView(children: [
          ListTile(
            title: Text(
              getCurrentTemperature(),
              style: TextStyle(fontSize: 50),
            ),
            subtitle: const Text("Current temperature"),
          ),
          ListTile(
            title: new GestureDetector(
              onTap: () {
                setTargetTemperature();
              },
              child: Text(
                getTargetTemperature(),
                style: TextStyle(fontSize: 50),
              ),
            ),
            subtitle: const Text("Target temperature"),
          ),
          ListTile(
              title: DropdownButton<Mode>(
                itemHeight: 60,
                value: _currentMode,
                //icon: Icon(Icons.arrow_downward),
                iconSize: 24,
                elevation: 16,
                style: TextStyle(fontSize: 50, color: Colors.black),
                underline: Container(
                  height: 2,
                  color: Color(0),
                ),
                onChanged: (Mode newValue) {
                  setMode(newValue);
                },
                items: <Mode>[
                  Mode.Off,
                  Mode.First,
                  Mode.Second,
                  Mode.Both,
                  Mode.Fan,
                ].map<DropdownMenuItem<Mode>>((Mode value) {
                  return DropdownMenuItem<Mode>(
                    value: value,
                    child: Text(getModeText(value)),
                  );
                }).toList(),
              ),
              subtitle: const Text("Mode")),
          new Divider(
            height: 15.0,
            color: Colors.grey,
          ),
          getEventTile(0),
          Divider(),
          getEventTile(1),
          Divider(),
          getEventTile(2),
          Divider(),
          getEventTile(3),
          Divider(),
          getEventTile(4),
          Divider(),
          getEventTile(5),
          Divider(),
          getEventTile(6),
          Divider(),
          getEventTile(7),
        ]),
      ),
    );
  }

  ListTile getEventTile(int number) {
    if (_events == null || _events[number] == null)
      return new ListTile(title: Text('$number: -'));

    return new ListTile(
        leading: _events[number].enabled
            ? new Image(
                image: AssetImage("assets/icons/timer.png"),
                width: 24,
                height: 24)
            : new Image(
                image: AssetImage("assets/icons/timer_disable.png"),
                width: 24,
                height: 24),
        title: Text(getEventText(_events[number])),
        onTap: () {
          editEvent(_events[number]);
        });
  }
}
