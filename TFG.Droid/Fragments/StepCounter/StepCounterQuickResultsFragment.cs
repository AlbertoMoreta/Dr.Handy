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
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.StepCounter {
    public class StepCounterQuickResultsFragment : Fragment, IHealthFragmentTabItem {

        public string Title { get; } = "Quick Results";

        private CustomTextView _stepsYesterday;
        private CustomTextView _caloriesYesterday;
        private CustomTextView _distanceYesterday;

        private CustomTextView _stepsLastWeek;
        private CustomTextView _caloriesLastWeek;
        private CustomTextView _distanceLastWeek; 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_stepcounter_body_quick_results, container, false);

            _stepsYesterday = view.FindViewById<CustomTextView>(Resource.Id.steps_yesterday);
            _caloriesYesterday = view.FindViewById<CustomTextView>(Resource.Id.calories_yesterday);
            _distanceYesterday = view.FindViewById<CustomTextView>(Resource.Id.distance_yesterday);

            _stepsLastWeek = view.FindViewById<CustomTextView>(Resource.Id.steps_last_week);
            _caloriesLastWeek = view.FindViewById<CustomTextView>(Resource.Id.calories_last_week);
            _distanceLastWeek = view.FindViewById<CustomTextView>(Resource.Id.distance_last_week);

            UpdateYesterdayInfo();
            UpdateLastWeekInfo(); 
            return view;
        }

        private void UpdateYesterdayInfo() {
            var yesterday =
                DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now.AddDays(-1));

            if (yesterday.Count > 0) {
                var item = yesterday.ElementAt(0);
                _stepsYesterday.Text = item.Steps.ToString();
                _caloriesYesterday.Text = item.Calories.ToString();
                _distanceYesterday.Text = item.Distance.ToString();
            } else {
                _stepsYesterday.Text = "-";
                _caloriesYesterday.Text = "-";
                _distanceYesterday.Text = "-";
            }


        }

        private void UpdateLastWeekInfo() {
            var lastWeek =
                DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now.AddDays(-7));

            if (lastWeek.Count > 0) {
                var item = lastWeek.ElementAt(0);
                _stepsLastWeek.Text = item.Steps.ToString();
                _caloriesLastWeek.Text = item.Calories.ToString();
                _distanceLastWeek.Text = item.Distance.ToString();
            } else {
                _stepsLastWeek.Text = "-";
                _caloriesLastWeek.Text = "-";
                _distanceLastWeek.Text = "-";
            }
        }
    }
}