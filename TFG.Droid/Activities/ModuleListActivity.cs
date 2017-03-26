using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using TFG.Droid.Adapters;
using TFG.Droid.Custom_Views;
using AnimationUtils = TFG.Droid.Utils.AnimationUtils;

namespace TFG.Droid {
    [Activity(Label = "ModuleListActivity", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask)]
    public class ModuleListActivity : BaseActivity {

        private ListView _modulesList; 

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            var theme = Resources.GetIdentifier("AppTheme_purple", "style", PackageName);
            if (theme != -1) { SetTheme(theme); }

            SetContentView(Resource.Layout.modules_list);
            SetUpToolBar(false);

            HealthModulesListAdapter adapter = new HealthModulesListAdapter(this);
            
            _modulesList = FindViewById<ListView>(Resource.Id.listView);
            _modulesList.Adapter = adapter;

        }

        
    }
}