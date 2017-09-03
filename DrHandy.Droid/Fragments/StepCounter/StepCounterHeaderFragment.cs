using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DrHandy.DataBase;
using DrHandy.Droid.Custom_Views;
using DrHandy.Droid.Interfaces;
using DrHandy.Droid.Listeners;
using DrHandy.Droid.Services;
using System.Globalization;

namespace DrHandy.Droid.Fragments.StepCounter {
    /// <summary>
    /// Header fragment for the Step Counter health module
    /// </summary>
    public class StepCounterHeaderFragment : Fragment, IHealthFragment, IStepDetectedListener {

        public bool IsBound { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        public StepCounterServiceConnection _serviceConnection;
        public bool _firstRun = true; 

        private CustomTextView _steps;
        private CustomTextView _calories;
        private CustomTextView _distance;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            if (!Utils.StepCounterUtils.IsKitKatWithStepCounter(Activity.PackageManager)) {
                Console.WriteLine("The device is not compatible with the step sensor");
                return;
            } 

            StartStepCounterService();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_stepcounter_header, container, false); 

            _steps = view.FindViewById<CustomTextView>(Resource.Id.steps);
            _calories = view.FindViewById<CustomTextView>(Resource.Id.calories);
            _distance = view.FindViewById<CustomTextView>(Resource.Id.distance);
            UpdateSteps();

            SetHasOptionsMenu(true);

            return view;
        }

        public override void OnResume() {
            base.OnResume();

            (Activity as BaseActivity).ToolbarTitle.Text = DateTime.Now.ToString("dddd, dd MMMM",
                    CultureInfo.CurrentCulture);

            _firstRun = false;
        }

        public void StepDetected() { 
            UpdateSteps();
        }

        private void UpdateSteps() {
#if DEBUG
            Console.WriteLine("Updating UI...");
#endif

            int stepsToday = 0;
            int caloriesToday = 0;
            double distanceToday = 0;

            if (Binder != null) {
                stepsToday = Binder.GetStepCounterService().Steps;
                caloriesToday = Binder.GetStepCounterService().Calories;
                distanceToday = Binder.GetStepCounterService().Distance;
            } else {
                var items =
                    DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now);

                if (items.Count > 0) {
                    var item = items.ElementAt(0);
                    stepsToday = item.Steps;
                    caloriesToday = item.Calories;
                    distanceToday = item.Distance;
                } 
            }

            _steps.Text = stepsToday.ToString();
            _calories.Text = caloriesToday.ToString();
            _distance.Text = distanceToday.ToString(); 
        }


        public override void OnStart() {
            base.OnStart();

            if (!Utils.StepCounterUtils.IsKitKatWithStepCounter(Activity.PackageManager)) {
                Console.WriteLine("The device is not compatible with the step sensor");
                return;
            }


            if (!_firstRun) StartStepCounterService();

            if (IsBound) return;

            BindService();
        }

        private void StartStepCounterService() {
            try {
#if DEBUG
                Console.WriteLine("Starting Step Counter Service...");
#endif
                var service = new Intent(Activity, typeof(StepCounterService));
                Activity.ApplicationContext.StartService(service); 
            } catch (Exception e) { }
        }

        private void BindService() {
            try {
#if DEBUG
                Console.WriteLine("Binding client to the service...");
#endif
                var serviceIntent = new Intent(Activity, typeof(StepCounterService));
                _serviceConnection = new StepCounterServiceConnection(this);
                Activity.ApplicationContext.BindService(serviceIntent, _serviceConnection, Bind.AutoCreate);
            } catch (Exception e) { }
        }

        private void UnbindService() {
#if DEBUG
            Console.WriteLine("Unbinding client from the service...");
#endif
            Activity.ApplicationContext.UnbindService(_serviceConnection);
            IsBound = false;
        }

        public override void OnDestroy() {
            base.OnDestroy(); 
            if (IsBound) {
                Binder.GetStepCounterService().RemoveListener(this);
                UnbindService();
            } 
        }

        public override void OnStop() {
            base.OnStop();
            if (IsBound) {
                Binder.GetStepCounterService().RemoveListener(this);
                UnbindService();
            } 
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
            inflater.Inflate(Resource.Menu.share_menu, menu);
            var scale = Resources.DisplayMetrics.Density;
            var toolBar = (Activity as BaseActivity).ToolBar;
            var paddingLeft = (int) (48 * scale + 0.5f);
            toolBar.SetPadding(paddingLeft, toolBar.PaddingTop, toolBar.PaddingRight, toolBar.PaddingBottom);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item) {
            switch (item.ItemId) {
                case Resource.Id.share:
                    var shareIntent = new Intent(Intent.ActionSend);
                    shareIntent.SetType("text/plain");
                    var shareText = Resources.GetString(Resource.String.share_steps, _steps.Text, _calories.Text, _distance.Text);
                    shareIntent.PutExtra(Intent.ExtraText, shareText);
                    StartActivity(Intent.CreateChooser(shareIntent, Activity.Resources.GetString(Resource.String.share)));
                    return true;
                default: return base.OnOptionsItemSelected(item);
            }
            
        }

    }
}