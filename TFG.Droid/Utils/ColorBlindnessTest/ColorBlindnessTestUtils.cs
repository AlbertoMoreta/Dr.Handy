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
using TFG.Droid.Fragments.ColorBlindnessTest;
using TFG.Droid.Interfaces;

namespace TFG.Droid.Utils {
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
    }
}