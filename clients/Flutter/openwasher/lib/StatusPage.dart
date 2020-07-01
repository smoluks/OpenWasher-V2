import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_i18n/flutter_i18n.dart';
import 'package:intl/intl.dart';

import 'Dto/WasherState.dart';
import 'Helpers/Dialogs.dart';
import 'Helpers/Notifications.dart';
import 'Managers/ProtocolManager.dart';

class StatusPage extends StatefulWidget {
  final WasherState state;

  const StatusPage(this.state);

  @override
  _StatusPage createState() => new _StatusPage();
}

class _StatusPage extends State<StatusPage> {
  WasherState _currentState;
  StreamSubscription<WasherState> _stateSubscription;

  @override
  void initState() {
    print('StatusPage creating...');

    super.initState();

    _currentState = widget.state;

    _stateSubscription =
        ProtocolManager.instance.onStateChange().listen(onStateChange);
  }

  Future<void> onStateChange(WasherState state) async {
    if (state.error != 0) {
      onError(state.error);
    }

    if (!state.isRunning) {
      await _stateSubscription?.cancel();

      await Notifications.showNotification(
          'Program ${FlutterI18n.translate(context, "program.${_currentState.program.index}.name")} complete',
          'Program complete');

      Navigator.of(context).pop(true);
    } else
      setState(() {
        _currentState = state;
      });
  }

  @override
  void dispose() {
    _stateSubscription?.cancel();

    super.dispose();
  }

  onError(int error) {
    Dialogs.showErrorBox(
        context,
        FlutterI18n.translate(context, "common.errorTextBox") +
            error.toRadixString(16), () {
      Navigator.of(context).pop(false);
    });
  }

  String getStartTime() {
    var startTime = new DateTime.fromMillisecondsSinceEpoch(
        DateTime.now().millisecondsSinceEpoch - _currentState.programPastTime);
    var formatter = new DateFormat('HH:mm:ss');

    return formatter.format(startTime);
  }

  String getStopTime() {
    var startTime = new DateTime.fromMillisecondsSinceEpoch(
        DateTime.now().millisecondsSinceEpoch +
            _currentState.programFullTime -
            _currentState.programPastTime);
    var formatter = new DateFormat('HH:mm:ss');

    return formatter.format(startTime);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Washing..."),
        actions: <Widget>[Icon(Icons.bluetooth_connected)],
      ),
      body: Container(child: new ListView(children: getListTiles())),
    );
  }

  List<Widget> getListTiles() {
    var result = new List<Widget>();

    result.add(getProgramPanel());
    result.add(getStagePanel());
    result.add(getIconPanel());

    if (_currentState.programFullTime != null &&
        _currentState.programPastTime != null) {
      result.add(getProgressPanel());
    }

    result.add(getStopButton());

    return result;
  }

  Widget getProgramPanel() {
    return ListTile(
      title: Text(
        FlutterI18n.translate(
            context, "program.${_currentState.program.index}.name"),
        style: TextStyle(fontSize: 30),
      ),
      subtitle: Text(FlutterI18n.translate(context, "common.program")),
    );
  }

  Widget getStagePanel() {
    return ListTile(
      title: Text(
        FlutterI18n.translate(
            context, "stage.${_currentState.stage.index}.name"),
        style: TextStyle(fontSize: 30),
      ),
      subtitle: Text(FlutterI18n.translate(context, "common.stage")),
    );
  }

  Widget getIconPanel() {
    return ListTile(
        title: Row(children: [
      Icon(
        Icons.rotate_left,
      ),
      Text('${_currentState.speed / 16} rps'),
      SizedBox(
        width: 20.0,
      ),
      Icon(
        Icons.donut_small,
      ),
      Text('${_currentState.temperature}°'),
    ]));
  }

  Widget getProgressPanel() {
    return ListTile(
      title: LinearProgressIndicator(
          value: _currentState.programPastTime / _currentState.programFullTime),
      subtitle: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [Text(getStartTime()), Text(getStopTime())]),
    );
  }

  Widget getStopButton() {
    return ListTile(
        title: RaisedButton(
      shape: new RoundedRectangleBorder(
          borderRadius: new BorderRadius.circular(18.0),
          side: BorderSide(color: Colors.blue)),
      color: Colors.blue,
      textColor: Colors.white,
      padding: EdgeInsets.all(8.0),
      onPressed: () async {
        await Dialogs.showAcceptBox(context, 'Really stop?', () async {
          try {
            await ProtocolManager.instance.stopProgram();
          } catch (ex) {
            print('stop error: $ex');
          }
        });
      },
      child: Text(
        "Stop",
        style: TextStyle(
          fontSize: 20.0,
        ),
      ),
    ));
  }
}
