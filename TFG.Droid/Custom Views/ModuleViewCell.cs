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

namespace TFG.Droid.Custom_Views {
    class ModuleViewCell : RelativeLayout{

        private TextView _moduleName;
        private TextView _moduleDescription;
        private ImageView _moduleIcon;
        private ImageView _addButton;

        public string Name {
            set { _moduleName.Text = value; }
            get { return _moduleName.Text; }
        }

        public string Description {
            set { _moduleDescription.Text = value; }
            get { return _moduleDescription.Text; }
        }

        public Drawable IconDrawable {
            set { _moduleIcon.SetImageDrawable(value); }
            get { return _moduleIcon.Drawable; }

        }

        public ImageView AddButton{
            get { return _addButton; }
        }

        public Drawable AddButtonImage {
            set { _addButton.SetImageDrawable(value); }
            get { return _addButton.Drawable; }
        }


        public ModuleViewCell(Context context) : base(context) {
            Init();
        } 

        private void Init() {
            var inflater = LayoutInflater.From(Context);
            inflater.Inflate(Resource.Layout.module_viewcell, this);

            _moduleName = FindViewById<TextView>(Resource.Id.module_name);
            _moduleDescription = FindViewById<TextView>(Resource.Id.module_description);
            _moduleIcon = FindViewById<ImageView>(Resource.Id.module_icon);
            _addButton = FindViewById<ImageView>(Resource.Id.module_addbutton);
        }

    }
}