using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using TFG.Droid.Activities;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Droid.Utils;
using TFG.Model;
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.StepCounter {
    public class StepCounterChartFragment : Fragment{  

        private DateTime _date = DateTime.Now;
        private ChartUtils.VisualizationMetric _metric;
        private CustomTextView _dateText;
        private string[] _labels;

        private CardViewBarChart _stepsChart;
        private CardViewBarChart _caloriesChart;
        private CardViewBarChart _distanceChart;

        public StepCounterChartFragment(ChartUtils.VisualizationMetric metric) {
            _metric = metric;
            

        } 
        public StepCounterChartFragment(ChartUtils.VisualizationMetric metric, DateTime date) : this(metric) {
            _date = date;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {  
            var view =  inflater.Inflate(Resource.Layout.fragment_stepcounter_body_chart_results, container, false); 

            //Chart color
            var colorName =
                HealthModulesInfo.GetHealthModuleColorFromHealthModuleName(
                    ((ModuleDetailActivity) Activity).CurrentHealthModule);
            var colorRes = ContextCompat.GetColor(Activity, Activity.Resources.GetIdentifier(colorName, "color", Activity.PackageName));

            //X Axis labels
            _labels = _metric == ChartUtils.VisualizationMetric.Weekly
                ? Resources.GetStringArray(Resource.Array.week_labels)
                : Resources.GetStringArray(Resource.Array.year_labels);

            _stepsChart = view.FindViewById<CardViewBarChart>(Resource.Id.steps_chart);
            _stepsChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("steps", "string", Activity.PackageName));
            _stepsChart.Color = colorRes; 

            _caloriesChart = view.FindViewById<CardViewBarChart>(Resource.Id.calories_chart);
            _caloriesChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("calories", "string", Activity.PackageName));
            _caloriesChart.Color = colorRes; 

            _distanceChart = view.FindViewById<CardViewBarChart>(Resource.Id.distance_chart);
            _distanceChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("distance", "string", Activity.PackageName));
            _distanceChart.Color = colorRes; 

            RefreshCharts();

            var colorFilter = new PorterDuffColorFilter(new Color(colorRes), PorterDuff.Mode.Multiply); //Arrows color
            var previousBtn = view.FindViewById<ImageView>(Resource.Id.previous);
            previousBtn.Click += ChangeDate;
            previousBtn.Drawable.SetColorFilter(colorFilter);
            var nextBtn = view.FindViewById<ImageView>(Resource.Id.next);
            nextBtn.Click += ChangeDate;
            nextBtn.Drawable.SetColorFilter(colorFilter);
            _dateText = view.FindViewById<CustomTextView>(Resource.Id.date_text); 
            RefreshDateText();

            return view;
        }

        private void ChangeDate(object sender, EventArgs eventArgs) {

            if (((ImageView) sender).Id == Resource.Id.previous) {
                _date = _metric == ChartUtils.VisualizationMetric.Weekly ? _date.AddDays(-7) : _date.AddYears(-1); 

            } else {
                _date = _metric == ChartUtils.VisualizationMetric.Weekly ? _date.AddDays(7) : _date.AddYears(1);

            }

            RefreshDateText();
            RefreshCharts();
            
        }

        //Refresh Data of charts
        private void RefreshCharts() {
            var itemsForMetric = ChartUtils.GetStepCounterItemsFromMetric(_metric, _date);
            _stepsChart.PopulateChart(ChartUtils.StepCounter_StepsToBarEntries(itemsForMetric, _labels.Length), _labels);
            _caloriesChart.PopulateChart(ChartUtils.StepCounter_CaloriesToBarEntries(itemsForMetric, _labels.Length), _labels); 
            _distanceChart.PopulateChart(ChartUtils.StepCounter_DistanceToBarEntries(itemsForMetric, _labels.Length), _labels);

            _stepsChart.Chart.NotifyDataSetChanged();
            _caloriesChart.Chart.NotifyDataSetChanged();
            _distanceChart.Chart.NotifyDataSetChanged();

            _stepsChart.Chart.Invalidate();
            _caloriesChart.Chart.Invalidate();
            _distanceChart.Chart.Invalidate();
        }

        //Refresh date text for charts
        private void RefreshDateText() {
            switch (_metric) {
                case ChartUtils.VisualizationMetric.Weekly: { 
                    var startDate = _date.AddDays(0 - (int) _date.DayOfWeek);
                    var endDate = _date.AddDays(6 - (int) _date.DayOfWeek);
                    _dateText.Text = startDate.ToString("dd MMMM") + " - " + endDate.ToString("dd MMMM");
                    _dateText.TextSize = (int) Resources.GetDimension(Resource.Dimension.text_size_large) /
                                         Resources.DisplayMetrics.Density;
                    break;
                }
                case ChartUtils.VisualizationMetric.Yearly:
                    _dateText.Text = _date.Year.ToString();
                    _dateText.TextSize = (int) Resources.GetDimension(Resource.Dimension.text_size_xlarge)/
                                         Resources.DisplayMetrics.Density;
                    break;
            }
        }

        public override void SetMenuVisibility(bool menuVisible) {
            base.SetMenuVisibility(menuVisible);
            if (menuVisible) {
                _stepsChart.Chart.AnimateY(700);
                _caloriesChart.Chart.AnimateY(700);
                _distanceChart.Chart.AnimateY(700);
            }
        }
    }
} 