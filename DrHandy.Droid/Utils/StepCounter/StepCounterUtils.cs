using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DrHandy.DataBase;
using DrHandy.Droid.Fragments.StepCounter;
using DrHandy.Droid.Interfaces;
using DrHandy.Model;
using DrHandy.Droid.Services;

namespace DrHandy.Droid.Utils {
    class StepCounterUtils : HealthModuleUtils {

        public override void InitModuleDB()  {
            DBHelper.Instance.CreateStepCounterTable(); 
        }
        public override void DeviceBootUp(Context context, string shortName) {
            //Start StepCounter Service
            var stepCounterIntent = new Intent(context, typeof(StepCounterService));
            context.StartService(stepCounterIntent);
        } 

        public override IHealthFragment GetHeaderFragment() {
            return new StepCounterHeaderFragment();
        }

        public override IHealthFragment GetBodyFragment() {
            return new StepCounterBodyFragment();
        }

        public override IHealthFragment GetHealthCardFragment(string shortName) {
            return new StepCounterCardFragment(shortName);
        } 

        public static bool IsKitKatWithStepCounter(PackageManager pm) {

            // Require at least Android KitKat
            int currentApiVersion = (int)Build.VERSION.SdkInt;

            // Check if API version is higher or equal than KitKat 
            // and the device supports StepDetector sensor
            return currentApiVersion >= 19
                && pm.HasSystemFeature(PackageManager.FeatureSensorStepDetector);

        }

        public override NotificationItem GetNotificationItem(Context context, HealthModule healthModule) {
            throw new NotImplementedException();
        }

    }
}