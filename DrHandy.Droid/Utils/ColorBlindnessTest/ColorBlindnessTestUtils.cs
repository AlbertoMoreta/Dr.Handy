using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DrHandy.Droid.Fragments.ColorBlindnessTest;
using DrHandy.Droid.Interfaces;
using DrHandy.Model;

namespace DrHandy.Droid.Utils {
    class ColorBlindnessTestUtils : HealthModuleUtils  {
        public override void InitModuleDB() { }

        public override Drawable GetHealthModuleIcon(Context context) {
            return GetDrawableFromResources(context, "colorblindnesstest_icon");
        }

        public override IHealthFragment GetHeaderFragment() {
            return  new CBTHeaderFragment();
        }

        public override IHealthFragment GetBodyFragment() {
            return new CBTBodyFragment();
        }

        public override IHealthFragment GetHealthCardFragment(string name) {
            return null;
        }

        public override NotificationItem GetNotificationItem(Context context, HealthModule healthModule) {
            throw new NotImplementedException();
        }
    }
}