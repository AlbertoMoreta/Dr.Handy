using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using TFG.Droid.Fragments.ColorBlindnessTest;
using TFG.Droid.Interfaces;
using TFG.Model;

namespace TFG.Droid.Activities {
    [Activity(Label = "ModuleDetailActivity", LaunchMode = LaunchMode.SingleTask)]
    public class ModuleDetailActivity : BaseActivity {

        public IHealthFragment HeaderFragment { get; private set; }
        public IHealthFragment BodyFragment { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.module_detail); 

            SetUpToolBar();

            var moduleName = Intent.GetStringExtra("name");
            ToolbarTitle.Text = moduleName;

            var moduleColorSufix = HealthModulesInfo.GetHealthModuleColorFromHealthModuleName(moduleName);
            if (moduleColorSufix == null) { moduleColorSufix = "purple"; }

            Window.DecorView.Background = ContextCompat.GetDrawable(this, 
                                                Resources.GetIdentifier("background_" + moduleColorSufix, 
                                                "drawable", PackageName));

            var theme = Resources.GetIdentifier("AppTheme_" + moduleColorSufix, "style", PackageName);
            if (theme != -1) { SetTheme(theme);} 

            FragmentManager fragmentManager = FragmentManager;
            FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

            HeaderFragment = HealthModulesInfoExtension.GetHeaderFragmentFromHealthModuleName(moduleName);
            if (HeaderFragment != null) {
                fragmentTransaction.Add(Resource.Id.fragments_container, HeaderFragment as Fragment);
            }

            BodyFragment = HealthModulesInfoExtension.GetBodyFragmentFromHealthModuleName(moduleName);
            if (BodyFragment != null) {
                fragmentTransaction.Add(Resource.Id.fragments_container, BodyFragment as Fragment);
            }

            fragmentTransaction.Commit();
        } 
    }
}
