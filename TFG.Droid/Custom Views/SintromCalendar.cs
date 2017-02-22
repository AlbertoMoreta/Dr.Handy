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

namespace TFG.Droid.Custom_Views {
    class SintromCalendar : LinearLayout {

        public SintromCalendar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Init(); } 
        public SintromCalendar(Context context) : base(context) { Init(); } 
        public SintromCalendar(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); } 
        public SintromCalendar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(); } 
        public SintromCalendar(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) {  Init();}

        private void Init() {
            var inflater = LayoutInflater.From(Context);
            inflater.Inflate(Resource.Layout.sintrom_calendar, this);
        }
    }
}