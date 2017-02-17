using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using com.refractored;
using TFG.Droid.Adapters;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Droid.Utils;

namespace TFG.Droid.Fragments.StepCounter {
    class StepCounterBodyFragment: Fragment, IHealthFragment {

        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_tabs, container, false);

            var pager = view.FindViewById<ViewPager>(Resource.Id.pager); 
            var adapter = new HealthModulePagerAdapter(((AppCompatActivity) Activity).SupportFragmentManager);
            adapter.AddItem(new StepCounterQuickResultsFragment(), "Quick Results");
            var weeklyResultsTitle = Activity.GetString(Activity.Resources.GetIdentifier("weekly_results",
                "string", Activity.PackageName));
            adapter.AddItem(new StepCounterChartFragment(ChartUtils.VisualizationMetric.Weekly), weeklyResultsTitle);
            var yearlyResultsTitle = Activity.GetString(Activity.Resources.GetIdentifier("yearly_results",
                "string", Activity.PackageName));
            adapter.AddItem(new StepCounterChartFragment(ChartUtils.VisualizationMetric.Yearly), yearlyResultsTitle);
            pager.Adapter = adapter;

            var tabs = view.FindViewById<PagerSlidingTabStrip>(Resource.Id.tabs);
            tabs.SetViewPager(pager);
            tabs.TabTextColor = ColorStateList.ValueOf(Color.AntiqueWhite);
            tabs.TabTextColorSelected = ColorStateList.ValueOf(Color.White);
            tabs.IndicatorColor = Color.White;

           

            return view;
        }


        
    }
}