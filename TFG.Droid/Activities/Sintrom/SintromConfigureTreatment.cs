using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using com.refractored;
using TFG.Droid.Adapters;
using TFG.Droid.Custom_Views;
using TFG.Droid.Fragments.Sintrom;
using TFG.Model;
using DayOfWeek = Android.Text.Format.DayOfWeek;

namespace TFG.Droid.Activities.Sintrom {
    
    [Activity(Label = "SintromConfigureTreatment", LaunchMode = LaunchMode.SingleTask)]
    public class SintromConfigureTreatment : BaseActivity {
         
        protected override void OnCreate(Bundle savedInstanceState)  {
            base.OnCreate(savedInstanceState); 
            SetUpToolBar();

            var moduleName = HealthModuleType.Sintrom.HealthModuleName();
            var theme = HealthModulesInfoExtension.GetHealthModuleThemeFromHealthModuleName(this, moduleName);
            if (theme != -1) { SetTheme(theme);} 

            Window.DecorView.Background =
                HealthModulesInfoExtension.GetHealthModuleBackgroundFromHealthModuleName(this, moduleName);
  

            SetContentView (Resource.Layout.sintrom_configure_treatment);

            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adapter = new HealthModulePagerAdapter(SupportFragmentManager);  

             var treatmentTitle = GetString(Resources.GetIdentifier("sintrom_treatment",
                "string", PackageName));
            adapter.AddItem(new SintromCalendarFragment(DateTime.Now), treatmentTitle);

            pager.Adapter = adapter;

            InitDays();
        }

        private void InitDays() {
            
            var names = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;

            for (var i = 0; i < names.Length; i++) {
                FindViewById<CustomTextView>(Resources.GetIdentifier("day" + i, "id", PackageName)).Text = names[i];
            }  
             

        }
        
    }
}