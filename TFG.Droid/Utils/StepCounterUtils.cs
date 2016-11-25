using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TFG.Droid.Utils {
    class StepCounterUtils {

        public static bool IsKitKatWithStepCounter(PackageManager pm) {

            // Require at least Android KitKat
            int currentApiVersion = (int) Build.VERSION.SdkInt;

            // Check if API version is higher or equal than KitKat 
            // and the device supports StepDetector sensor
            return currentApiVersion >= 19 
                && pm.HasSystemFeature(PackageManager.FeatureSensorStepDetector);

        }
    }
}