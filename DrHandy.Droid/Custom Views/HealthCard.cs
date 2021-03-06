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
using Android.Util;
using DrHandy.Model;

namespace DrHandy.Droid.Custom_Views {
    
    /*
     * HealthCard - Custom view for the health cards
     */ 
    class HealthCard : SquareLinearLayout{ 

        public string Name { set; get; }
        public Drawable Icon { set; get; }

        public HealthModule HealthModule { get; set; }

         
        public HealthCard(Context context, HealthModule module) : base(context) {
            HealthModule = module;
            Init();
        }

        public HealthCard(Context context, IAttributeSet attrs, HealthModule module) : base(context, attrs) {
            HealthModule = module;
            Init();
        }


        private void Init() {
            var inflater = LayoutInflater.From(Context);
            inflater.Inflate(Resource.Layout.health_card, this); 

        }
         
         



    }
}