using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
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

        public static void AnimateIcon(View v, int x2, int y2, float scaleFactor){

            //Translate Animation
            var path = new Path();
            var x1 = v.GetX();
            var y1 = v.GetY();

            path.MoveTo(x1, y1);
            path.QuadTo(x1,y1,x2,y2);

            var pathAnimator = ObjectAnimator.OfFloat(v, "x", "y", path);

            pathAnimator.SetDuration(500);  
            pathAnimator.AnimationEnd += delegate { v.SetX(x2); v.SetY(y2);};

            //Scale Animation
            var scaleAnimatorX = ObjectAnimator.OfFloat(v, "scaleX", scaleFactor);
            var scaleAnimatorY = ObjectAnimator.OfFloat(v, "scaleY", scaleFactor); 
            

            pathAnimator.Start();
            scaleAnimatorX.Start();
            scaleAnimatorY.Start();


        } 


    }
}