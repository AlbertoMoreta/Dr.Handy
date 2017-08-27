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
using Android.Util;
using Android.Views;
using Android.Widget;
using com.refractored;
using DrHandy.Droid.Adapters;
using DrHandy.Droid.Interfaces;
using DrHandy.Droid.Utils;

namespace DrHandy.Droid.Fragments.Sintrom {
    public class SintromBodyFragment : Fragment, IHealthFragment { 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_tabs, container, false);

            var pager = view.FindViewById<ViewPager>(Resource.Id.pager);
            var adapter = new HealthModulePagerAdapter(((AppCompatActivity) Activity).SupportFragmentManager);

            var treatmentTitle = Activity.GetString(Activity.Resources.GetIdentifier("sintrom_treatment",
                "string", Activity.PackageName));
            adapter.AddItem(new SintromTreatmentFragment(), treatmentTitle);
            var inrTitle = Activity.GetString(Activity.Resources.GetIdentifier("sintrom_inr",
                "string", Activity.PackageName));
            adapter.AddItem(new SintromResultsFragment(), inrTitle);

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