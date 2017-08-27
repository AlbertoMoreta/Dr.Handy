using System;
using System.Globalization;
using Android.Graphics;
using Android.OS; 
using Android.Support.V4.Content; 
using Android.Views;
using Android.Widget;
using DrHandy.DataBase;
using DrHandy.Droid.Activities;
using DrHandy.Droid.Custom_Views; 
using DrHandy.Droid.Utils;
using DrHandy.Model; 
using Fragment = Android.Support.V4.App.Fragment;

namespace DrHandy.Droid.Fragments.StepCounter {
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
            var colorName = DBHelper.Instance.GetHealthModuleByShortName(((ModuleDetailActivity) Activity).CurrentHealthModule.ShortName).Color;
            var colorRes = ContextCompat.GetColor(Activity, Activity.Resources.GetIdentifier(colorName, "color", Activity.PackageName));
             

            //X Axis labels
            if (_metric == ChartUtils.VisualizationMetric.Weekly) {
                //Reorganize days based on current culture
                var days = CultureInfo.CurrentCulture.DateTimeFormat.ShortestDayNames;
                string[] currentCultureDays = new string[7];
                for(var i = 0; i < days.Length; i++) {
                    currentCultureDays[i] = days[((int) CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek + i) % days.Length];
                }
                _labels = currentCultureDays;

            } else { 
                _labels = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            } 
            

            _stepsChart = view.FindViewById<CardViewBarChart>(Resource.Id.steps_chart);
            _stepsChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("steps", "string", Activity.PackageName));
            _stepsChart.Color = colorRes; 

            _caloriesChart = view.FindViewById<CardViewBarChart>(Resource.Id.calories_chart);
            _caloriesChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("calories", "string", Activity.PackageName));
            _caloriesChart.Color = colorRes; 

            _distanceChart = view.FindViewById<CardViewBarChart>(Resource.Id.distance_chart);
            _distanceChart.Title.Text = Activity.GetString(Activity.Resources.GetIdentifier("distance", "string", Activity.PackageName));
            _distanceChart.Color = colorRes; 

            RefreshCharts(true);

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
            RefreshCharts(true);
            
        }

        //Refresh Data of charts
        public void RefreshCharts(bool animate = false) {
            if (_stepsChart != null && _caloriesChart != null && _distanceChart != null) {
                var itemsForMetric = ChartUtils.GetStepCounterItemsFromMetric(_metric, _date);
                _stepsChart.PopulateChart(
                    ChartUtils.StepCounter_StepsToBarEntries(itemsForMetric, _metric, _labels.Length), _labels);
                _caloriesChart.PopulateChart(
                    ChartUtils.StepCounter_CaloriesToBarEntries(itemsForMetric, _metric, _labels.Length), _labels);
                _distanceChart.PopulateChart(
                    ChartUtils.StepCounter_DistanceToBarEntries(itemsForMetric, _metric, _labels.Length), _labels);

                if (animate) {
                    _stepsChart.Chart.AnimateY(700);
                    _caloriesChart.Chart.AnimateY(700);
                    _distanceChart.Chart.AnimateY(700);
                }

                _stepsChart.Chart.NotifyDataSetChanged();
                _caloriesChart.Chart.NotifyDataSetChanged();
                _distanceChart.Chart.NotifyDataSetChanged();

                _stepsChart.Chart.Invalidate();
                _caloriesChart.Chart.Invalidate();
                _distanceChart.Chart.Invalidate();
            }
        }

        //Refresh date text for charts
        private void RefreshDateText() {
            switch (_metric) {
                case ChartUtils.VisualizationMetric.Weekly: {
                    var firstDay = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                    var startDate = _date.AddDays(-((int) _date.DayOfWeek - (int)firstDay) % 7);
                    var endDate = startDate.AddDays(6);
                    _dateText.Text = startDate.ToString("dd MMM") + " - " + endDate.ToString("dd MMM");
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
                _stepsChart?.Chart.AnimateY(700);
                _caloriesChart?.Chart.AnimateY(700);
                _distanceChart?.Chart.AnimateY(700);
            }
        }
    }
} 