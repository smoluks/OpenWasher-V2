import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter_i18n/flutter_i18n.dart';

import 'Dto/Events.dart';
import 'Dto/OpenWasherDevice.dart';
import 'Dto/Program.dart';
import 'Dto/WasherState.dart';
import 'Helpers/Dialogs.dart';
import 'Managers/ProtocolManager.dart';
import 'ParametersPage.dart';
import 'StatusPage.dart';

class SelectProgramPage extends StatefulWidget {
  final OpenWasherDevice device;

  const SelectProgramPage({this.device});

  @override
  _SelectProgramPage createState() => new _SelectProgramPage();
}

class _SelectProgramPage extends State<SelectProgramPage> {
  bool _loading = true;
  StreamSubscription<WasherState> _stateSubscription;

  @override
  void initState() {
    super.initState();

    connect();
  }

  Future<void> connect() async {
    try {
      //connect
      await ProtocolManager.instance
          .connect(widget.device.address, onClose, onError, onEvent);

      //subscribe to state change
      _stateSubscription =
          ProtocolManager.instance.onStateChange().listen(onStateChange);
    } catch (ex) {
      print('Connection error: $ex');

      await Dialogs.showErrorBox(context, 'Connection error: ${ex.message}',
          () {
        Navigator.of(context).pop(false);
      });
    }
  }

  onClose() {
    Navigator.of(context).pop(false);
  }

  onError(int error) {
    Dialogs.showErrorBox(
        context,
        FlutterI18n.translate(context, "common.errorTextBox") +
            error.toRadixString(16), () {
      Navigator.of(context).pop(false);
    });
  }

  onEvent(int event) {
    switch (event) {
      case Events.GOTOBOOTLOADER:
        Dialogs.showErrorBox(
            context, FlutterI18n.translate(context, "common.bootloaderTextBox"),
            () {
          Navigator.of(context).pop(false);
        });
        break;
    }
  }

  Future<void> onStateChange(WasherState state) async {
    if (state.error != 0) {
      onError(state.error);
    }

    if (state.isRunning) {
      await _stateSubscription?.cancel();

      await Navigator.of(context).push(MaterialPageRoute(builder: (context) {
        return StatusPage(state);
      }));

      _stateSubscription =
          ProtocolManager.instance.onStateChange().listen(onStateChange);
    }

    setState(() {
      _loading = false;
    });
  }

  @override
  void dispose() {
    print('SelectProgramPage destroying...');

    _stateSubscription?.cancel();
    ProtocolManager.instance.disconnect();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.device.name ?? '-'),
        actions: <Widget>[
          _loading
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
              : Icon(Icons.bluetooth_connected)
        ],
      ),
      body: Container(
        child: new ListView(
            children: ListTile.divideTiles(
                    context: context, tiles: getProgramTiles(context))
                .toList()),
      ),
    );
  }

  List<Widget> getProgramTiles(BuildContext context) {
    var tiles = new List<Widget>();

    for (Program value in Program.values) {
      tiles.add(ListTile(
        title: Text(
          FlutterI18n.translate(context, "program.${value.index}.name"),
          style: TextStyle(fontSize: 30),
        ),
        //subtitle: Text(
        //    FlutterI18n.translate(context, "program.${value.index}.comment")),
        enabled: !_loading,
        onTap: () async {
          await _stateSubscription?.cancel();

          await Navigator.of(context)
              .push(MaterialPageRoute(builder: (context) {
            return ParametersPage(value);
          }));

          _stateSubscription =
              ProtocolManager.instance.onStateChange().listen(onStateChange);

          setState(() {
            _loading = false;
          });
        },
      ));
    }

    return tiles;
  }
}
