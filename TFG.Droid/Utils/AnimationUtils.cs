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

namespace TFG.Droid.Utils {
    class AnimationUtils {

        public static void RevealViewCircular(View v, int centerX, int centerY, int radius) { 
            var anim = ViewAnimationUtils.CreateCircularReveal(v, centerX, centerY,
              0, radius);

            anim.SetDuration(500);
            v.Visibility = ViewStates.Visible;
            anim.Start();
        }


        public static void HideViewCircular(View v, int centerX, int centerY, int radius) { 
            var anim = ViewAnimationUtils.CreateCircularReveal(v, centerX, centerY,
              radius, 0);

            anim.SetDuration(500);
            anim.Start();
            anim.AnimationEnd += delegate { v.Visibility = ViewStates.Gone; };


        }

    }
}