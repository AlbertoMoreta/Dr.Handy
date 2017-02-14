using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;

namespace TFG.Droid.Fragments.StepCounter {
    public class StepCounterChartFragment : Fragment, IHealthFragmentTabItem {
        public string Title { get; }

        private Chart _chart;
        private DateTime _date = DateTime.Now;

        public StepCounterChartFragment(Chart.VisualizationMetric metric) {

            switch (metric)
            {
                case Chart.VisualizationMetric.Weekly:
                    Title = Activity.GetString(Activity.Resources.GetIdentifier("weekly_results", "string", Activity.PackageName));
                    break;
                case Chart.VisualizationMetric.Yearly:
                    Title = Activity.GetString(Activity.Resources.GetIdentifier("yearly_results", "string", Activity.PackageName));
                    break;
            }

        } 
        public StepCounterChartFragment(Chart.VisualizationMetric metric, DateTime date) : this(metric) {
            _date = date;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_stepcounter_body_quick_results, container, false);

            _chart = view.FindViewById<Chart>(Resource.Id.chart); 

            return view;
        }

        private void UpdateYesterdayInfo() {
            var yesterday =
                DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now.AddDays(-1)); 


        } 
    }
} 