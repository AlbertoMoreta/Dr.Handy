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

namespace TFG.Droid.Receivers {
    [BroadcastReceiver]
     [IntentFilter(new[] { Intent.ActionMain })]
    public class NotificationReceiver : BroadcastReceiver { 

        public override void OnReceive(Context context, Intent intent) {
            var moduleName = intent.GetStringExtra("moduleName");

            if
            var title = intent.GetStringExtra("title");
            var description = intent.GetStringExtra("description");
            var audio = intent.GetBooleanExtra("audio", false);
            var requestCode = intent.GetIntExtra("requestCode", -1);

            var clickIntent = new Intent(context, typeof(ModuleDetailActivity));
            clickIntent.PutExtra("name", moduleName);
            var contentIntent = PendingIntent.GetActivity(context, requestCode, clickIntent, PendingIntentFlags.CancelCurrent);

            var manager = NotificationManagerCompat.From(context);

            var style = new NotificationCompat.BigTextStyle();
            style.BigText(description);

            var wearableExtender = new NotificationCompat.WearableExtender()
                .SetBackground(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.Icon));


            var builder = new NotificationCompat.Builder(context)
                .SetContentIntent(contentIntent)
                .SetAutoCancel(true)
                .SetContentTitle(title)
                .SetContentText(description)
                .SetStyle(style)
                .SetWhen(JavaSystem.CurrentTimeMillis())
                .Extend(wearableExtender)
                .SetSmallIcon(Resource.Drawable.Icon); 

            if (audio) {
                builder.SetSound(Android.Media.RingtoneManager.GetDefaultUri(Android.Media.RingtoneType.Notification));
            }

            var notification = builder.Build();
            manager.Notify(0, notification);
        }
    }
}