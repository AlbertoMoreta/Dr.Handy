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
using MikePhil.Charting.Listener;
using DrHandy.Droid.Utils;

namespace DrHandy.Droid.Custom_Views {
    class CardViewBarChart : CardView, GestureDetector.IOnGestureListener, View.IOnTouchListener {

        private Context _context;
        private GestureDetector _gestureDetector;

        public BarChart Chart { get; set; }
        public CustomTextView Title { get; set; }

        private int _color;
        public int Color {
            get { return _color; }
            set {

                if (Chart != null && Chart.BarData != null) {
                    foreach (BarDataSet dataSet in Chart.BarData.DataSets)  {
                        dataSet.Color = value;
                    }
                    Chart.Invalidate();
                }

                _color = value;
            }
        }

        public CardViewBarChart(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Init(); }
        public CardViewBarChart(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(); _context = context; }
        public CardViewBarChart(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); _context = context; }
        public CardViewBarChart(Context context) : base(context) { Init(); _context = context; }

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
            Chart.SetOnTouchListener(this); 
            _gestureDetector = new GestureDetector(_context, this);


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
                Chart.AxisLeft.AxisMaximum *= (float) 1.3;
                Chart.AxisLeft.AxisMinimum /= (float) 1.3; 
                if (Chart.AxisLeft.AxisMinimum < 0) { Chart.AxisLeft.AxisMinimum = 0; }


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

          
        public bool OnDown(MotionEvent e) { return true; }
        //Prevent parent to steal horizontal scroll events 
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) {
            return true;
        } 
        public void OnLongPress(MotionEvent e) {  }
        //Prevent parent to steal horizontal scroll events 
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) {
            if (Math.Abs(e1.GetX() - e2.GetX()) > Math.Abs(e1.GetY() - e2.GetY())) {
                Chart.Parent.RequestDisallowInterceptTouchEvent(true);
            }
            return true;
        } 
        public void OnShowPress(MotionEvent e) {} 
        public bool OnSingleTapUp(MotionEvent e) {
            return true;
        } 
        public bool OnTouch(View v, MotionEvent e) {
            _gestureDetector.OnTouchEvent(e);
            return false;
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