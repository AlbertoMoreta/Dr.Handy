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
using Java.Lang;
using TFG.DataBase;
using TFG.Droid.Interfaces;
using TFG.Model;

namespace TFG.Droid.Activities {
    [Activity(Label = "ModuleDetailActivity", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ModuleDetailActivity : BaseActivity {
         
        public HealthModule CurrentHealthModule { get; private set; }
        public IHealthFragment HeaderFragment { get; private set; }
        public IHealthFragment BodyFragment { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.module_detail); 

            var shortName = Intent.GetStringExtra("ShortName");
            var healthModule = DBHelper.Instance.GetHealthModuleByShortName(shortName);

            CurrentHealthModule = healthModule; 
            SetUpToolBar(showLoginInfo: healthModule.LoginRequired);
            ToolbarTitle.Text = healthModule.Name;

            Window.DecorView.Background = healthModule.GetBackground(this); 

            var theme = healthModule.GetTheme(this); 
            if (theme != -1) { SetTheme(theme);} 

            FragmentManager fragmentManager = FragmentManager;
            FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

            HeaderFragment = healthModule.GetHeaderFragment();
            BodyFragment = healthModule.GetBodyFragment();
            if (savedInstanceState == null) {   //Prevent the fragments to duplicate
                if (HeaderFragment != null)  {
                    fragmentTransaction.Add(Resource.Id.fragments_container, HeaderFragment as Fragment);
                }

                if (BodyFragment != null) {
                    fragmentTransaction.Add(Resource.Id.fragments_container, BodyFragment as Fragment);
                }

                fragmentTransaction.Commit();
            }
        } 
    }
}
