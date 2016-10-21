using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TFG.Droid.Adapters;

namespace TFG.Droid {
    [Activity(Label = "ModuleListActivity", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask)]
    public class ModuleListActivity : BaseActivity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.modules_list);
            SetUpToolBar();

            ModuleViewCellAdapter adapter = new ModuleViewCellAdapter(this);
            
            ListView modulesList = FindViewById<ListView>(Resource.Id.listView);
            modulesList.Adapter = adapter;
            

        }
    }
}