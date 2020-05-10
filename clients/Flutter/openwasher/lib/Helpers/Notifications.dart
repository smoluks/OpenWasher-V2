import 'package:flutter_local_notifications/flutter_local_notifications.dart';

class Notifications {
  static FlutterLocalNotificationsPlugin flutterLocalNotificationsPlugin;

  static Future<void> showNotification(String text, String header) async {
    var androidPlatformChannelSpecifics = AndroidNotificationDetails(
        '1', 'openwasher', 'openwasher',
        importance: Importance.Max, priority: Priority.High, ticker: 'ticker');
    var iOSPlatformChannelSpecifics = IOSNotificationDetails();
    var platformChannelSpecifics = NotificationDetails(
        androidPlatformChannelSpecifics, iOSPlatformChannelSpecifics);

    await flutterLocalNotificationsPlugin
        .show(0, header, text, platformChannelSpecifics, payload: 'item x');
  }
}
