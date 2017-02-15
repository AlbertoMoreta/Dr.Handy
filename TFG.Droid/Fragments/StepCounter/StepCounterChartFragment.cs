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
using TFG.Droid.Interfaces;
using TFG.Droid.Utils; 
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.StepCounter {
    public class StepCounterChartFragment : Fragment{ 

        private BarChart _chart;
        private DateTime _date = DateTime.Now;
        private ChartUtils.VisualizationMetric _metric;

        public StepCounterChartFragment(ChartUtils.VisualizationMetric metric) {
            _metric = metric;
            

        } 
        public StepCounterChartFragment(ChartUtils.VisualizationMetric metric, DateTime date) : this(metric) {
            _date = date;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

            View view = null;
            try {
                view =  inflater.Inflate(Resource.Layout.fragment_stepcounter_body_chart_results, container, false); 

                _chart = view.FindViewById<BarChart>(Resource.Id.chart);
                _chart.SetDrawBarShadow(true);
                _chart.SetDrawGridBackground(false);
                _chart.Legend.Enabled = false;
                _chart.SetDrawValueAboveBar(true);
                _chart.HighlightPerDragEnabled = false;
                _chart.HighlightPerTapEnabled = false;
                _chart.ScaleYEnabled = false;
                _chart.AnimateY(700);
                _chart.ScaleX = 1;
                _chart.Description.Text = "";

                //X Axis Properties
                var xAxis = _chart.XAxis;
                xAxis.Position = XAxis.XAxisPosition.Bottom;
                xAxis.SetDrawGridLines(false);

                //Left Axis Properties
                var leftAxis = _chart.AxisLeft;
                leftAxis.AxisMaximum = 100f;
                leftAxis.AxisMinimum = 0f;
                leftAxis.SetDrawGridLines(false);
                leftAxis.SetLabelCount(5, true);

                //Disable Right Axis
                var rightAxis = _chart.AxisRight;
                rightAxis.Enabled = false;
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