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

namespace TFG.Droid.Activities {
    [Activity(Label = "ModuleDetailActivity", Theme = "@style/AppTheme")]
    public class ModuleDetailActivity : BaseActivity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.module_detail);
            SetUpToolBar();

            ToolbarTitle.Text = Intent.GetStringExtra("name");
        } 
    }
}