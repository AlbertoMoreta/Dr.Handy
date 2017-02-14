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
using Java.Lang;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Droid.Utils;
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.StepCounter {
    public class StepCounterChartFragment : Fragment, IHealthFragmentTabItem {
        public string Title { get; private set; }

        private Chart _chart;
        private DateTime _date = DateTime.Now;
        private Chart.VisualizationMetric _metric;

        public StepCounterChartFragment(Chart.VisualizationMetric metric) {
            _metric = metric;

            

        } 
        public StepCounterChartFragment(Chart.VisualizationMetric metric, DateTime date) : this(metric) {
            _date = date;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

            View view = null;
            try {
                view =  inflater.Inflate(Resource.Layout.fragment_stepcounter_body_weekly_results, container, false);

                Title =
                    Activity.GetString(Activity.Resources.GetIdentifier(_metric.ToString().ToLower() + "_results",
                        "string", Activity.PackageName));

                _chart = view.FindViewById<Chart>(Resource.Id.chartView);
                _chart.PopulateChart(ChartUtils.StepCounter_StepsToBarEntries(_metric, DateTime.Now), _metric);
            }catch(NullPointerException npe) { }

            return view;
        }

        private void UpdateYesterdayInfo() {
            var yesterday =
                DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now.AddDays(-1)); 


        } 
    }
} 