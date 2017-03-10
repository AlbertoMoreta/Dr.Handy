using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using TFG.Droid.Activities;
using TFG.Model;

namespace TFG.Droid.Receivers {
    [BroadcastReceiver]
    public class NotificationReceiver : BroadcastReceiver { 

        public override void OnReceive(Context context, Intent intent) {
            var moduleId = intent.GetIntExtra("moduleId", -1); 
            var requestCode = intent.GetIntExtra("requestCode", -1);


            var notificationItem = HealthModulesInfo.GetHealthModuleTypeById(moduleId).GetNotificationItem();

            if (notificationItem != null) {
                var clickIntent = new Intent(context, typeof(ModuleDetailActivity));
                clickIntent.PutExtra("name", moduleId);
                var contentIntent = PendingIntent.GetActivity(context, requestCode, clickIntent,
                    PendingIntentFlags.CancelCurrent);

                var manager = NotificationManagerCompat.From(context);

                var style = new NotificationCompat.BigTextStyle();
                style.BigText(notificationItem.Description);

                var wearableExtender = new NotificationCompat.WearableExtender()
                    .SetBackground(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.Icon));


                var builder = new NotificationCompat.Builder(context)
                    .SetContentIntent(contentIntent)
                    .SetAutoCancel(true)
                    .SetContentTitle(notificationItem.Title)
                    .SetContentText(notificationItem.Description)
                    .SetStyle(style)
                    .SetWhen(JavaSystem.CurrentTimeMillis())
                    .Extend(wearableExtender)
                    .SetSmallIcon(Resource.Drawable.Icon);

                if (notificationItem.Audio) {
                    builder.SetSound(Android.Media.RingtoneManager.GetDefaultUri(Android.Media.RingtoneType.Notification));
                }

                var notification = builder.Build();
                manager.Notify(0, notification);
            }
        }
    }
}