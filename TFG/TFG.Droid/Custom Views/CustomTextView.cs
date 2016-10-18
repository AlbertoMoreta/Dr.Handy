using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TFG.Droid.Custom_Views {
    class CustomTextView : TextView{
        public CustomTextView(Context context) : base(context) {
            Init();
        }

        public CustomTextView(Context context, IAttributeSet attrs) : base(context, attrs) {
            Init();
        }



        private void Init()  {
            Typeface = Typeface.CreateFromAsset(Context.Assets, "Fonts/DroidSerif-Bold.ttf");
        }
    }
}