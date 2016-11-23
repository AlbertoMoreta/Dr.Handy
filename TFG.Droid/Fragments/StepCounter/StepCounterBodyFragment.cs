using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;

namespace TFG.Droid.Fragments.StepCounter {
    class StepCounterBodyFragment: Fragment, IHealthFragment {

        private CustomTextView _stepsYesterday;
        private CustomTextView _stepsLastWeek;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_stepcounter_body, container, false);

            _stepsYesterday = view.FindViewById<CustomTextView>(Resource.Id.steps_yesterday);
            _stepsLastWeek = view.FindViewById<CustomTextView>(Resource.Id.steps_last_week);
            UpdateStepsYesterday();
            UpdateStepsLastWeek();
            return view;
        }


        private void UpdateStepsYesterday() {
            var yesterday =
                DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now.AddDays(-1));

            _stepsYesterday.Text = yesterday.Count > 0
                ? yesterday.ElementAt(0).Steps.ToString()
                : "-";
        }

        private void UpdateStepsLastWeek()  {
            var lastWeek =
                DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now.AddDays(-7));

            _stepsLastWeek.Text = lastWeek.Count > 0
                ? lastWeek.ElementAt(0).Steps.ToString()
                : "-";
        }
    }
}