
using Android.Animation;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views; 

namespace DrHandy.Droid.Utils{

    /*
     * AnimationUtils - Utils class with methods for animating views
     */
    class AnimationUtils {

        public static void RevealViewCircular(View v, int centerX, int centerY, int radius, long duration = 500, long delay = 0) {

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
                var anim = ViewAnimationUtils.CreateCircularReveal(v, centerX, centerY, 0, radius);

                anim.SetDuration(duration);
                anim.StartDelay = delay;
                anim.Start();

            }

            v.Visibility = ViewStates.Visible;
        }

        public static void HideViewCircular(View v, int centerX, int centerY, int radius, long duration = 500, long delay = 0) {


            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) {
                var anim = ViewAnimationUtils.CreateCircularReveal(v, centerX, centerY, radius, 0);

                anim.SetDuration(duration);
                anim.StartDelay = delay;
                anim.Start();
                anim.AnimationEnd += delegate { v.Visibility = ViewStates.Gone; };

            } else {
                v.Visibility = ViewStates.Gone;
            }

        } 

        public static void StartTranslateAnimation(View v, int endX, int endY, long duration = 500, long delay = 0) {

            //Translate Animation
            var path = new Path();
            var startX = v.GetX();
            var startY = v.GetY();

            path.MoveTo(startX, startY);
            path.QuadTo(startX, startY, endX, endY);

            var translateAnimatorX = ObjectAnimator.OfFloat(v, "x", endX);
            var translateAnimatorY = ObjectAnimator.OfFloat(v, "y", endY);

            translateAnimatorX.SetDuration(duration);
            translateAnimatorY.SetDuration(duration);

            translateAnimatorX.StartDelay = delay;
            translateAnimatorY.StartDelay = delay;

            translateAnimatorX.Start();
            translateAnimatorY.Start();  

        }

        public static void StartScaleAnimation(View v, int width, int height, long duration = 500, long delay = 0) {
            //Scale Animation
            var scaleAnimatorX = ObjectAnimator.OfInt(v, "scaleX", width);
            var scaleAnimatorY = ObjectAnimator.OfInt(v, "scaleY", height);

            scaleAnimatorX.SetDuration(duration);
            scaleAnimatorX.StartDelay = delay;
            scaleAnimatorY.SetDuration(duration);
            scaleAnimatorY.StartDelay = delay;

            scaleAnimatorX.Start();
            scaleAnimatorY.Start();
        }

        public static void StartScaleAnimation(View v, float scaleFactorX, float scaleFactorY, long duration = 500, long delay = 0) {
            //Scale Animation
            var scaleAnimatorX = ObjectAnimator.OfFloat(v, "scaleX", scaleFactorX);
            var scaleAnimatorY = ObjectAnimator.OfFloat(v, "scaleY", scaleFactorY);

            scaleAnimatorX.SetDuration(duration);
            scaleAnimatorX.StartDelay = delay;
            scaleAnimatorY.SetDuration(duration);
            scaleAnimatorY.StartDelay = delay;

            scaleAnimatorX.Start();
            scaleAnimatorY.Start();
        }

        public static void FadeAnimation(View v, float alpha, long duration = 500, long delay = 0) {
            //Fade Animation   
            var fadeAnimator = ObjectAnimator.OfFloat(v, "alpha", alpha);
            fadeAnimator.SetDuration(duration);
            fadeAnimator.StartDelay = delay;  
            fadeAnimator.AnimationStart += delegate { if(alpha != 0) {  v.Visibility = ViewStates.Visible;} };
            fadeAnimator.AnimationEnd += delegate { if(alpha == 0) { v.Visibility = ViewStates.Gone;} };
            fadeAnimator.Start();
        } 

        public static ValueAnimator ExpandView(View v, int finalHeight, bool wrapContent = false, long duration = 500, long delay = 0) {
            var expandAnimator = ValueAnimator.OfInt(v.MeasuredHeight, finalHeight);
            expandAnimator.Update += (s, e) => { 
            
                var val = e.Animation.AnimatedFraction == 1 && wrapContent
                    ? ViewGroup.LayoutParams.WrapContent
                    : (int) e.Animation.AnimatedValue; 

                 ViewGroup.LayoutParams lp = v.LayoutParameters;
                 lp.Height = val;
                 v.LayoutParameters = lp;
            };

            expandAnimator.SetDuration(duration);
            expandAnimator.StartDelay = delay; 

            return expandAnimator;
        }
    }
}