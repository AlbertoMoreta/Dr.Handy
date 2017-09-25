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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using DrHandy.Droid.Adapters;
using DrHandy.Droid.Custom_Views;
using DrHandy.Logic;
using AnimationUtils = DrHandy.Droid.Utils.AnimationUtils;

namespace DrHandy.Droid {
    
    /*
     *  ModuleListActivity - This activity will list all the existing health modules in the app 
     */
    [Activity(Label = "@string/tools_activity_name", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ModuleListActivity : BaseActivity {

        private RecyclerView _modulesList;
        private RecyclerView.Adapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState); 

            SetContentView(Resource.Layout.modules_list);
            SetUpToolBar(false, true); 
            
            
            _modulesList = FindViewById<RecyclerView>(Resource.Id.recycler_view);
            _modulesList.HasFixedSize = true; //TODO

            _layoutManager = new LinearLayoutManager(this);
            _modulesList.SetLayoutManager(_layoutManager);

            _adapter = new HealthModulesListAdapter(this, HealthModulesConfigReader.GetHealthModules());
            _modulesList.SetAdapter(_adapter); 
            
        } 


    }
}