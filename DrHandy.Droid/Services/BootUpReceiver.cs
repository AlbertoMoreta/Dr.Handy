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

namespace DrHandy.Droid.Services {
    [BroadcastReceiver]
    [IntentFilter(new [] {Intent.ActionBootCompleted, Intent.ActionMyPackageReplaced})]
    class BootUpReceiver : BroadcastReceiver{
        public override void OnReceive(Context context, Intent intent) {
            var stepCounterIntent = new Intent(context, typeof(StepCounterService));
            context.StartService(stepCounterIntent);
        }
    }
}