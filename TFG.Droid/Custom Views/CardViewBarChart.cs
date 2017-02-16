using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using TFG.Droid.Utils;

namespace TFG.Droid.Custom_Views {
    class CardViewBarChart : CardView {

        private Context _context;
        private int _color;
        public BarChart Chart { get; set; }
        public CustomTextView Title { get; set; } 
        public int Color {
            get { return _color; }
            set {

                if (Chart != null && Chart.BarData != null) {
                    foreach (DataSet dataSet in Chart.BarData.DataSets)  {
                        dataSet.Color = value;
                    }
                    Chart.Invalidate();
                }

                _color = value;
            }
        }

        public CardViewBarChart(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Init(); }
        public CardViewBarChart(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) {
            _context = context;
            Init();
        }
        public CardViewBarChart(Context context, IAttributeSet attrs) : base(context, attrs) {
            _context = context;
            Init();
        }
        public CardViewBarChart(Context context) : base(context) {
            _context = context;
            Init();
        }

        private void Init() {
            var inflater = LayoutInflater.From(Context);
            inflater.Inflate(Resource.Layout.cardview_barchart, this);

            Title = FindViewById<CustomTextView>(Resource.Id.title);

            Chart = FindViewById<BarChart>(Resource.Id.chart);
            Chart.SetDrawBarShadow(true);
            Chart.SetDrawGridBackground(false);
            Chart.Legend.Enabled = false;
            Chart.SetDrawValueAboveBar(true);
            Chart.HighlightPerDragEnabled = false;
            Chart.HighlightPerTapEnabled = false;
            Chart.ScaleYEnabled = false;
            Chart.AnimateY(700);
            Chart.ScaleX = 1;
            Chart.Description.Text = "";

            //X Axis Properties
            var xAxis = Chart.XAxis;
            xAxis.Position = XAxis.XAxisPosition.Bottom;
            xAxis.Granularity = 1f;
            xAxis.SetDrawGridLines(false);

            //Left Axis Properties
            var leftAxis = Chart.AxisLeft;
            leftAxis.SetDrawGridLines(false);

            //Disable Right Axis
            var rightAxis = Chart.AxisRight;
            rightAxis.Enabled = false; 
        }

        public void PopulateChart(List<BarEntry> barEntries, string[] labels = null) {
            if (Chart != null)  {

                Chart.AxisLeft.AxisMaximum = barEntries.Select(x => x.GetY()).Max();    //Maximum Y axis value
                Chart.AxisLeft.AxisMinimum = barEntries.Select(x => x.GetY()).Min();    //Minimum Y axis value

                var barDataSet = new BarDataSet(barEntries, "");
                barDataSet.Color = _color; 
                Chart.Data = new BarData(barDataSet);
                if (labels != null) {
                    Chart.XAxis.ValueFormatter = new CustomAxisValueFormatter(labels);
                }
                Chart.NotifyDataSetChanged();
                Chart.Invalidate();
            }
        }
    }

    //Value formater for XAxis labels
    public class CustomAxisValueFormatter : IndexAxisValueFormatter {

        private readonly string[] _labels;

        public CustomAxisValueFormatter(string[] labels) {
            _labels = labels;
        }

        public override string GetFormattedValue(float value, AxisBase axis) {
            return _labels[(int) value];
        } 
        
    }
}