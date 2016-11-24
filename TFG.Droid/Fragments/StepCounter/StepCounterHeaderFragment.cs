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
using TFG.Droid.Listeners;
using TFG.Droid.Services;

namespace TFG.Droid.Fragments.StepCounter {
    /// <summary>
    /// Header fragment for the Step Counter health module
    /// </summary>
    class StepCounterHeaderFragment : Fragment, IHealthFragment {

        public bool IsBound { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        public StepCounterServiceConnection _serviceConnection;
        public bool _firstRun = true;
        private Handler _handler;

        private CustomTextView _steps;
        private CustomTextView _calories;
        private CustomTextView _distance;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState); 
            StartStepCounterService(); 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_stepcounter_header, container, false);
            _handler = new Handler();
            _steps = view.FindViewById<CustomTextView>(Resource.Id.steps);
            _calories = view.FindViewById<CustomTextView>(Resource.Id.calories);
            _distance = view.FindViewById<CustomTextView>(Resource.Id.distance);
            UpdateSteps();
            (Activity as BaseActivity).ToolbarTitle.Text = DateTime.Now.ToString("dd / MM / yyyy"); 

            return view;
        }

        private void UpdateSteps() {
#if DEBUG
            Console.WriteLine("Updating UI...");
#endif

            int stepsToday;

            if (Binder != null) {
                stepsToday = Binder.GetStepCounterService().Steps; 
            } else {
                var items =
                    DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now);

                stepsToday = items.Count > 0    //Check if item exists
                    ? items.ElementAt(0).Steps
                    : 0;
            }

            _steps.Text = stepsToday.ToString();
            _calories.Text = (stepsToday/20).ToString();
            _distance.Text = Math.Round((stepsToday * 0.00075), 2).ToString();
            _handler.PostDelayed(() => UpdateSteps(), 500); //Update the UI every 0.5 seconds
        }


        public override void OnStart() {
            base.OnStart();

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
                UnbindService();
            }
        }

        public override void OnStop() {
            base.OnStop();
            if (IsBound) {
                UnbindService();
            }

            _handler.RemoveCallbacksAndMessages(null);
        }

        public override void OnResume() {
            base.OnResume();
            if (!_firstRun) {
                if(_handler == null) {  _handler = new Handler();}
                _handler.PostDelayed(() => UpdateSteps(), 500);
            }

            _firstRun = false;
        }
         
    }
}