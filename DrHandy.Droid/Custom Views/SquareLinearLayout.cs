using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DrHandy.Droid.Custom_Views {
    /*
     * SquareLinearLayout - Simple LinearLayout with square shape
     */ 
    class SquareLinearLayout : LinearLayout {
        public SquareLinearLayout(Context context) : base(context) { } 
        public SquareLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs) { } 
        public SquareLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec) {
            base.OnMeasure(widthMeasureSpec, widthMeasureSpec);
            var width = MeasureSpec.GetSize(widthMeasureSpec);
            SetMeasuredDimension(width, width);
        }
    }
}