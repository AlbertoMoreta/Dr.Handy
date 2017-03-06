using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using TFG.Droid.Receivers;

namespace TFG.Droid.Utils {
    class NotificationsUtils {

        public static void ScheduleNotification(Context ctx, string moduleName, string title, string description, long time, bool audio) {
            var alarmIntent = new Intent(ctx, typeof(NotificationReceiver));
            alarmIntent.PutExtra("moduleName", moduleName);
            alarmIntent.PutExtra("title", title);
            alarmIntent.PutExtra("description", description);
            alarmIntent.PutExtra("audio", audio); 

            var pendingIntent = PendingIntent.GetBroadcast(ctx, 1, alarmIntent, PendingIntentFlags.CancelCurrent);

            var alarmManager = (AlarmManager) ctx.GetSystemService(Context.AlarmService); 
            alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + time, AlarmManager.IntervalDay, pendingIntent);
        }
    }
}