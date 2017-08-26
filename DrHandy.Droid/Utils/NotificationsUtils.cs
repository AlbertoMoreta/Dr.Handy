
using Android.App;
using Android.Content;
using Android.OS;
using DrHandy.Droid.Receivers;
using DrHandy.Model;

namespace DrHandy.Droid.Utils {
    class NotificationsUtils {

        public static void ScheduleNotification(Context ctx, string moduleShortName, long time) {
            var alarmIntent = new Intent(ctx, typeof(NotificationReceiver));
            alarmIntent.PutExtra("ShortName", moduleShortName);

            var pendingIntent = PendingIntent.GetBroadcast(ctx, 0, alarmIntent, PendingIntentFlags.CancelCurrent);

            var alarmManager = (AlarmManager) ctx.GetSystemService(Context.AlarmService); 
            alarmManager.SetRepeating(AlarmType.RtcWakeup, time, AlarmManager.IntervalDay, pendingIntent); 
        }
    }
}
