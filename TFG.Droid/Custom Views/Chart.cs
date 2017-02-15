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
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;

namespace TFG.Droid.Custom_Views {
    public class Chart : RelativeLayout {

        public enum VisualizationMetric {
            Weekly, Yearly
        }

        private Context _context;
        public BarChart ChartView { get; set; } 
        public CustomTextView XLabel { get; set; }
        public CustomTextView YLabel { get; set; }

        public Chart(Context context) : base(context) { _context = context; Init(); } 
        public Chart(Context context, IAttributeSet attrs) : base(context, attrs) { _context = context; Init(); } 

        private void Init() {
            LayoutInflater inflater = LayoutInflater.From(Context);
            View v = inflater.Inflate(Resource.Layout.chart_layout , this);

            ChartView = v.FindViewById<BarChart>(Resource.Id.chart);
            XLabel = v.FindViewById<CustomTextView>(Resource.Id.xLabel);
            YLabel = v.FindViewById<CustomTextView>(Resource.Id.yLabel);

            //Chart Properties
            ChartView.SetDrawBarShadow(true);
            ChartView.SetDrawGridBackground(false);
            ChartView.Legend.Enabled = false;
            ChartView.SetDrawValueAboveBar(true);
            ChartView.HighlightPerDragEnabled = false;
            ChartView.HighlightPerTapEnabled = false;
            ChartView.ScaleYEnabled = false;
            ChartView.AnimateY(700);
            ChartView.ScaleX = 1;
            ChartView.Description.Text = "";

            //X Axis Properties
            var xAxis = ChartView.XAxis;
            xAxis.Position = XAxis.XAxisPosition.Bottom;
            xAxis.SetDrawGridLines(false); 

            //Left Axis Properties
            var leftAxis = ChartView.AxisLeft;
            leftAxis.AxisMaximum = 100f;
            leftAxis.AxisMinimum = 0f; 
            leftAxis.SetDrawGridLines(false);
            leftAxis.SetLabelCount(5, true);

            //Disable Right Axis
            var rightAxis = ChartView.AxisRight;
            rightAxis.Enabled = false; 
        }

        public void PopulateChart(List<BarEntry> values, VisualizationMetric metric)  { 

            if (ChartView != null) { 
                ChartView.Data = new BarData(new BarDataSet(values, ""));
                ChartView.AnimateY(700);
                ChartView.NotifyDataSetChanged();
                ChartView.Invalidate();
            }

            var label = "";
            switch (metric) {
                case VisualizationMetric.Weekly: label = "weekday"; break; 
                case VisualizationMetric.Yearly: label = "month"; break;
            } 

            if (XLabel != null) { XLabel.Text = _context.GetString(_context.Resources.GetIdentifier(label, "string", _context.PackageName)); }

        } 
    }
}