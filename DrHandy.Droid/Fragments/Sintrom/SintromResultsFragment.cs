using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using DrHandy.DataBase;
using DrHandy.Droid.Activities;
using DrHandy.Droid.Custom_Views;
using DrHandy.Model;
using Fragment = Android.Support.V4.App.Fragment;

namespace DrHandy.Droid.Fragments.Sintrom {
    public class SintromResultsFragment : Fragment {
        private LineChart Chart { get; set; }
        private LinearLayout _inrInfo;
        private RelativeLayout _emptyINR;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) { 
            
            var view =  inflater.Inflate(Resource.Layout.fragment_sintrom_results, container, false);

            _inrInfo = view.FindViewById<LinearLayout>(Resource.Id.inr_info);
            _emptyINR = view.FindViewById<RelativeLayout>(Resource.Id.sintrom_empty_inr);

            Chart = view.FindViewById<LineChart>(Resource.Id.chart);
            Chart.SetDrawGridBackground(false);
            Chart.Legend.Enabled = false;
            Chart.HighlightPerDragEnabled = false;
            Chart.HighlightPerTapEnabled = false;   
            Chart.SetPinchZoom(true);
            Chart.Description.Text = ""; 
            Chart.ViewPortHandler.SetMaximumScaleX(5f);
            Chart.ViewPortHandler.SetMaximumScaleY(5f);

            var xAxis = Chart.XAxis; 
            xAxis.Position = XAxis.XAxisPosition.Bottom;  

            PopulateChart();

            return view;
        }

         private void PopulateChart() {
            if (Chart != null) {

                var lineEntries = GetLineEntries();

                if (lineEntries.Count > 0) {
                    _inrInfo.Visibility = ViewStates.Visible;
                    _emptyINR.Visibility = ViewStates.Gone;

                    Chart.AxisLeft.AxisMaximum = lineEntries.Select(x => x.Key.GetY()).Max(); //Maximum Y axis value
                    Chart.AxisLeft.AxisMinimum = lineEntries.Select(x => x.Key.GetY()).Min(); //Minimum Y axis value
                    Chart.AxisLeft.AxisMaximum *= (float) 1.3;
                    Chart.AxisLeft.AxisMinimum /= (float) 1.3;
                    if (Chart.AxisLeft.AxisMinimum < 0)
                    {
                        Chart.AxisLeft.AxisMinimum = 0;
                    }
                    

                    var lineDataSet = new LineDataSet(lineEntries.Keys.ToList(), "");
                    var colorName = DBHelper.Instance.GetHealthModuleByShortName(((ModuleDetailActivity)Activity).CurrentHealthModule.ShortName).Color;
                    var colorRes = ContextCompat.GetColor(Activity, Activity.Resources.GetIdentifier(colorName, "color", Activity.PackageName)); 
                    lineDataSet.Color = colorRes;
                    lineDataSet.SetDrawFilled(true);
                    lineDataSet.SetDrawCircles(false);
                    lineDataSet.FillColor = colorRes;
                    Chart.Data = new LineData(lineDataSet);
                    if (lineEntries.Values != null)
                    {
                        Chart.XAxis.ValueFormatter = new CustomAxisValueFormatter(lineEntries.Values.ToArray());
                    }
                    Chart.NotifyDataSetChanged();
                    Chart.Invalidate();
                } else {
                    _inrInfo.Visibility = ViewStates.Gone;
                    _emptyINR.Visibility = ViewStates.Visible;
                }
            }
        }

        private Dictionary<Entry, string> GetLineEntries() {
            Dictionary<Entry, string> entries = new Dictionary<Entry, string>();
            var inrItems = DBHelper.Instance.GetSintromINRItems(); 

            for(var i = 0; i < inrItems.Count; i++) {
                if (inrItems[i].INR != 0) {
                    entries[new BarEntry(i, (float) inrItems.ElementAt(i).INR)] =
                        inrItems.ElementAt(i).Date.ToString("dd MMM");
                }
            }

            return entries;
        }

        public override void SetMenuVisibility(bool menuVisible)  {
            base.SetMenuVisibility(menuVisible);
            if (menuVisible && Chart != null) {
                Chart.AnimateX(1000);
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
            if (value.Equals((int) value) && (int) value < _labels.Length) {
                return _labels[(int) value];
            }

            return "";
        } 
        
    }
}