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
using TFG.DataBase;
using TFG.Droid.Fragments.StepCounter;
using TFG.Droid.Interfaces;

namespace TFG.Droid.Utils {
    class StepCounterUtils : HealthModuleUtils {

        public override void InitModuleDB()  {
            DBHelper.Instance.CreateStepCounterTable(); 
        }

        public override Drawable GetHealthModuleIcon(Context context) {
            return GetDrawableFromResources(context, "stepcounter_icon");
        } 

        public override IHealthFragment GetHeaderFragment() {
            return new StepCounterHeaderFragment();
        }

        public override IHealthFragment GetBodyFragment() {
            return new StepCounterBodyFragment();
        }

        public override IHealthFragment GetHealthCardFragment(string moduleName) {
            return new StepCounterCardFragment(moduleName);
        } 

        public static bool IsKitKatWithStepCounter(PackageManager pm) {

            // Require at least Android KitKat
            int currentApiVersion = (int)Build.VERSION.SdkInt;

            // Check if API version is higher or equal than KitKat 
            // and the device supports StepDetector sensor
            return currentApiVersion >= 19
                && pm.HasSystemFeature(PackageManager.FeatureSensorStepDetector);

        }


    }
}