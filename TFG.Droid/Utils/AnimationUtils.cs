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
using Android.Support.V4.View;
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

        public static void StartTranslateAnimation(View v, int endX, int endY) {

            //Translate Animation
            var path = new Path();
            var startX = v.GetX();
            var startY = v.GetY();

            path.MoveTo(startX, startY);
            path.QuadTo(startX, startY, endX, endY);

            var pathAnimator = ObjectAnimator.OfFloat(v, "x", "y", path);

            pathAnimator.SetDuration(500); 
            pathAnimator.Start();


        }

        public static void StartScaleAnimation(View v, int width, int height) {
            //Scale Animation
            var scaleAnimatorX = ObjectAnimator.OfInt(v, "scaleX", width);
            var scaleAnimatorY = ObjectAnimator.OfInt(v, "scaleY", height);

            scaleAnimatorX.Start();
            scaleAnimatorY.Start();
        }

        public static void StartScaleAnimation(View v, float scaleFactorX, float scaleFactorY) {
            //Scale Animation
            var scaleAnimatorX = ObjectAnimator.OfFloat(v, "scaleX", scaleFactorX);
            var scaleAnimatorY = ObjectAnimator.OfFloat(v, "scaleY", scaleFactorY); 

            scaleAnimatorX.Start();
            scaleAnimatorY.Start();
        }

        public static void FadeAnimation(View v, float alpha) {
            //Fade Animation   
            var fadeAnimator = ObjectAnimator.OfFloat(v, "alpha", alpha);
            fadeAnimator.SetDuration(500);  
            fadeAnimator.AnimationStart += delegate { if(alpha != 0) {  v.Visibility = ViewStates.Visible;} };
            fadeAnimator.AnimationEnd += delegate { if(alpha == 0) { v.Visibility = ViewStates.Gone;} };
            fadeAnimator.Start();
        } 

    }
}