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
using TFG.Droid.Custom_Views;
using TFG.Model;
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.Sintrom {
    public class SintromResultsFragment : Fragment {
        private LineChart Chart { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) { 
            
            var view =  inflater.Inflate(Resource.Layout.fragment_sintrom_results, container, false);
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

                    Chart.AxisLeft.AxisMaximum = lineEntries.Select(x => x.Key.GetY()).Max(); //Maximum Y axis value
                    Chart.AxisLeft.AxisMinimum = lineEntries.Select(x => x.Key.GetY()).Min(); //Minimum Y axis value
                    Chart.AxisLeft.AxisMaximum *= (float) 1.3;
                    Chart.AxisLeft.AxisMinimum /= (float) 1.3;
                    if (Chart.AxisLeft.AxisMinimum < 0) {
                        Chart.AxisLeft.AxisMinimum = 0;
                    }


                    var lineDataSet = new LineDataSet(lineEntries.Keys.ToList(), "");
                    var color = ContextCompat.GetColor(Activity,
                        Activity.Resources.GetIdentifier(HealthModuleType.Sintrom.HealthModuleColor(), "color",
                            Activity.PackageName));
                    lineDataSet.Color = color;
                    lineDataSet.SetDrawFilled(true);
                    lineDataSet.SetDrawCircles(false);
                    lineDataSet.FillColor = color;
                    Chart.Data = new LineData(lineDataSet);
                    if (lineEntries.Values != null) {
                        Chart.XAxis.ValueFormatter = new CustomAxisValueFormatter(lineEntries.Values.ToArray());
                    }
                    Chart.NotifyDataSetChanged();
                    Chart.Invalidate();
                }
            }
        }

        private Dictionary<Entry, string> GetLineEntries() {
            Dictionary<Entry, string> entries = new Dictionary<Entry, string>();
            var inrItems = DBHelper.Instance.GetSintromINRItems(); 

            for(var i = 0; i < inrItems.Count; i++) {
                entries[new BarEntry(i, (float) inrItems.ElementAt(i).INR)] = inrItems.ElementAt(i).Date.ToString("dd MMM");
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
            return _labels[(int) value];
        } 
        
    }
}