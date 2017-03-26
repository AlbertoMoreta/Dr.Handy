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
using Android.Support.V7.App;
using TFG.Droid.Custom_Views;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace TFG.Droid {
    [Activity(Label = "BaseActivity", Theme ="@style/AppTheme")]
    public class BaseActivity : AppCompatActivity{

        public Toolbar ToolBar { get; set; }
        public CustomTextView ToolbarTitle { get; set; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);  
        }


        protected void SetUpToolBar(bool isTransparent = true) { 
            var toolBar = ToolBar = FindViewById<Toolbar>(Resource.Id.toolbar);
            
            if (toolBar != null) {  
                SetSupportActionBar(toolBar);
                SupportActionBar.SetDisplayShowTitleEnabled(false);
                ToolbarTitle = toolBar.FindViewById<CustomTextView>(Resource.Id.title);
                ToolbarTitle.Text = Title;
                if (isTransparent) { toolBar.Background = null; }

            }
        }
    }
}