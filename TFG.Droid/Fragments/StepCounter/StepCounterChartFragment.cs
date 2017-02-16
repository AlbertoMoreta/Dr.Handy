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
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Droid.Utils; 
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.StepCounter {
    public class StepCounterChartFragment : Fragment{  
        private DateTime _date = DateTime.Now;
        private ChartUtils.VisualizationMetric _metric;

        public StepCounterChartFragment(ChartUtils.VisualizationMetric metric) {
            _metric = metric;
            

        } 
        public StepCounterChartFragment(ChartUtils.VisualizationMetric metric, DateTime date) : this(metric) {
            _date = date;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {  
            var view =  inflater.Inflate(Resource.Layout.fragment_stepcounter_body_chart_results, container, false);

            var itemsForMetric = ChartUtils.GetStepCounterItemsFromMetric(_metric, DateTime.Now);

            var stepsChart = view.FindViewById<CardViewBarChart>(Resource.Id.steps_chart);
            stepsChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("steps", "string", Activity.PackageName));
            stepsChart.PopulateChart(ChartUtils.StepCounter_StepsToBarEntries(itemsForMetric));

            var caloriesChart = view.FindViewById<CardViewBarChart>(Resource.Id.calories_chart);
            caloriesChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("calories", "string", Activity.PackageName));
            caloriesChart.PopulateChart(ChartUtils.StepCounter_CaloriesToBarEntries(itemsForMetric));

            var distanceChart = view.FindViewById<CardViewBarChart>(Resource.Id.distance_chart);
            distanceChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("kilometers", "string", Activity.PackageName));
            distanceChart.PopulateChart(ChartUtils.StepCounter_DistanceToBarEntries(itemsForMetric));

            return view;
        }
    }
} 