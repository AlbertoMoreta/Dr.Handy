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
using TFG.Droid.Custom_Views;
using TFG.Droid.Interfaces;
using TFG.Droid.Listeners;
using TFG.Droid.Services;

namespace TFG.Droid.Fragments.StepCounter {
    /// <summary>
    /// Header fragment for the Step Counter health module
    /// </summary>
    class StepCounterHeaderFragment : Fragment, IHealthFragment, StepDetectedListener {

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
            (Activity as BaseActivity).ToolbarTitle.Text = DateTime.Now.ToString("dd / MM / yyyy"); 

            return view;
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
                Binder.GetStepCounterService().SetListener(null);
                UnbindService();
            } 
        }

        public override void OnStop() {
            base.OnStop();
            if (IsBound) {
                Binder.GetStepCounterService().SetListener(null);
                UnbindService();
            } 
        }

        public override void OnResume() {
            base.OnResume(); 

            _firstRun = false;
        }

    }
}