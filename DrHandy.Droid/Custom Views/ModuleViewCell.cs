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
using Android.Graphics.Drawables;
using com.refractored.fab;

namespace DrHandy.Droid.Custom_Views {
    
    /*
     * ModuleViewCell - Custom view for the items in the available health modules list
     */ 
    class ModuleViewCell : RelativeLayout{

        private TextView _moduleName;
        private TextView _moduleDescriptionShort;
        private CustomTextView _moduleDescriptionLong;
        private ImageView _moduleIcon;
        private FloatingActionButton _addButton;

        public string Name {
            set { _moduleName.Text = value; }
            get { return _moduleName.Text; }
        }

        public string Description {
            set {
                _moduleDescriptionShort.Text = value;
                _moduleDescriptionLong.Text = value;
            }
            get { return _moduleDescriptionShort.Text; }
        }

        public Drawable IconDrawable {
            set { _moduleIcon.SetImageDrawable(value); }
            get { return _moduleIcon.Drawable; }

        }

        public FloatingActionButton AddButton{
            get { return _addButton; }
        } 


        public ModuleViewCell(Context context) : base(context) {
            Init();
        } 

        private void Init() {
            var inflater = LayoutInflater.From(Context);
            inflater.Inflate(Resource.Layout.module_viewcell, this);

            _moduleName = FindViewById<TextView>(Resource.Id.module_name);
            _moduleDescriptionShort = FindViewById<TextView>(Resource.Id.module_description_short);
            _moduleDescriptionLong = FindViewById<CustomTextView>(Resource.Id.module_description_long);
            _moduleIcon = FindViewById<ImageView>(Resource.Id.module_icon);
            _addButton = FindViewById<FloatingActionButton>(Resource.Id.module_addbutton);
        }

    }
}