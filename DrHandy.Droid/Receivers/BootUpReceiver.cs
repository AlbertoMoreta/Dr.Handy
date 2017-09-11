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
using DrHandy.DataBase;

namespace DrHandy.Droid.Receivers {
    [BroadcastReceiver]
    [IntentFilter(new [] {Intent.ActionBootCompleted, Intent.ActionMyPackageReplaced})]
    class BootUpReceiver : BroadcastReceiver{
        public override void OnReceive(Context context, Intent intent) {

            //Notify device boot up for every visible module

            var modules = DBHelper.Instance.GetModules().Where(x => x.Visible).ToList();
            foreach(var module in modules)  {
                module.GetUtilsClass().DeviceBootUp(context, module.ShortName);
            } 
        }
    }
}