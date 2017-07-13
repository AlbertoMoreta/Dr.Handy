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
using TFG.DataBase;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.StepCounter {
    public class StepCounterQuickResultsFragment : Fragment {

        private CustomTextView _yesterdayText;
        private CustomTextView _stepsYesterday;
        private CustomTextView _caloriesYesterday;
        private CustomTextView _distanceYesterday;

        private CustomTextView _lastWeekText;
        private CustomTextView _stepsLastWeek;
        private CustomTextView _caloriesLastWeek;
        private CustomTextView _distanceLastWeek; 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_stepcounter_body_quick_results, container, false);

            _yesterdayText = view.FindViewById<CustomTextView>(Resource.Id.yesterday);
            _stepsYesterday = view.FindViewById<CustomTextView>(Resource.Id.steps_yesterday);
            _caloriesYesterday = view.FindViewById<CustomTextView>(Resource.Id.calories_yesterday);
            _distanceYesterday = view.FindViewById<CustomTextView>(Resource.Id.distance_yesterday);

            _lastWeekText = view.FindViewById<CustomTextView>(Resource.Id.last_week);
            _stepsLastWeek = view.FindViewById<CustomTextView>(Resource.Id.steps_last_week);
            _caloriesLastWeek = view.FindViewById<CustomTextView>(Resource.Id.calories_last_week);
            _distanceLastWeek = view.FindViewById<CustomTextView>(Resource.Id.distance_last_week);

            UpdateYesterdayInfo();
            UpdateLastWeekInfo(); 
            return view;
        }

        private void UpdateYesterdayInfo() {
            var yesterdayDate = DateTime.Now.AddDays(-1);
            var yesterday =
                DBHelper.Instance.GetStepCounterItemFromDate(yesterdayDate);

            _yesterdayText.Text += "\n(" + yesterdayDate.ToString("d") + ")";

            if (yesterday.Count > 0) {
                var item = yesterday.ElementAt(0);
                _stepsYesterday.Text = item.Steps.ToString();
                _caloriesYesterday.Text = item.Calories.ToString();
                _distanceYesterday.Text = item.Distance.ToString();
            } else {
                _stepsYesterday.Text = "0";
                _caloriesYesterday.Text = "0";
                _distanceYesterday.Text = "0";
            }


        }

        private void UpdateLastWeekInfo() {

            var lastWeekDate = DateTime.Now.AddDays(-7);

            var lastWeek =
                DBHelper.Instance.GetStepCounterItemFromDate(lastWeekDate);

            _lastWeekText.Text += "\n(" + lastWeekDate.ToString("d") + ")";

            if (lastWeek.Count > 0) {
                var item = lastWeek.ElementAt(0);
                _stepsLastWeek.Text = item.Steps.ToString();
                _caloriesLastWeek.Text = item.Calories.ToString();
                _distanceLastWeek.Text = item.Distance.ToString();
            } else {
                _stepsLastWeek.Text = "0";
                _caloriesLastWeek.Text = "0";
                _distanceLastWeek.Text = "0";
            }
        }
    }
}