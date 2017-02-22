using System;
using System.Collections.Generic;
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
using TFG.Droid.Fragments.Sintrom;
using TFG.Model;

namespace TFG.Droid.Activities.Sintrom {
    
    [Activity(Label = "SintromConfigureTreatmentFragment", LaunchMode = LaunchMode.SingleTask)]
    public class SintromConfigureTreatmentFragment : AppCompatActivity {
         
        protected override void OnCreate(Bundle savedInstanceState)  {
            base.OnCreate(savedInstanceState);
             

            var theme = HealthModulesInfoExtension.GetHealthModuleThemeFromHealthModuleName(this, HealthModuleType.Sintrom.HealthModuleName());
            if (theme != -1) { SetTheme(theme);} 

            SetContentView (Resource.Layout.sintrom_configure_treatment);

            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adapter = new HealthModulePagerAdapter(SupportFragmentManager);  

             var treatmentTitle = GetString(Resources.GetIdentifier("sintrom_treatment",
                "string", PackageName));
            adapter.AddItem(new SintromCalendarFragment(), treatmentTitle);

            pager.Adapter = adapter;

            var tabs = FindViewById<PagerSlidingTabStrip>(Resource.Id.tabs);
            tabs.SetViewPager(pager); 
        } 

        
    }
}