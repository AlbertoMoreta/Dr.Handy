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
using Android.Util;
using TFG.Model;

namespace TFG.Droid.Custom_Views {
    class HealthCard : LinearLayout{

        public  TextView _moduleName;
        private ImageView _moduleImage;

        public string Name {
            set { _moduleName.Text = value; }
            get { return _moduleName.Text;  }
        }

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

            _moduleName = FindViewById<TextView>(Resource.Id.module_name);
            _moduleImage = FindViewById<ImageView>(Resource.Id.module_image);

        }
         
         



    }
}