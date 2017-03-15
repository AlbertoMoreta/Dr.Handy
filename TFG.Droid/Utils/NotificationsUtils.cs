
using Android.App;
using Android.Content;
using Android.OS;
using TFG.Droid.Receivers;
using TFG.Model;

namespace TFG.Droid.Utils {
    class NotificationsUtils {

        public static void ScheduleNotification(Context ctx, int moduleId, long time) {
            var alarmIntent = new Intent(ctx, typeof(NotificationReceiver));
            alarmIntent.PutExtra("moduleId", moduleId);

            var pendingIntent = PendingIntent.GetBroadcast(ctx, 0, alarmIntent, 0);

            var alarmManager = (AlarmManager) ctx.GetSystemService(Context.AlarmService);
            alarmManager.SetRepeating(AlarmType.ElapsedRealtime, time, AlarmManager.IntervalDay, pendingIntent);
        }
    }
}
