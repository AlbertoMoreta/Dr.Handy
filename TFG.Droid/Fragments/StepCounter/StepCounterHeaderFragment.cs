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
    class StepCounterHeaderFragment : Fragment, IHealthFragment {

        public bool IsBound { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        public StepCounterServiceConnection _serviceConnection;
        public bool _firstRun = true;
        private Handler _handler;

        private CustomTextView _steps;

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            Console.WriteLine("Fragment OnCreate Called"); 
            StartStepCounterService();

            _handler = new Handler();
            _handler.PostDelayed(() => UpdateSteps(), 500);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_cbt_header, container, false); 
            _steps = view.FindViewById<CustomTextView>(Resource.Id.info_text); 

            return view;
        }

        private void UpdateSteps() {
            if (Binder != null) {
                _steps.Text = Binder.GetStepCounterService().Steps.ToString();
                _handler.PostDelayed(() => UpdateSteps(), 500);
            }
        }
     

        public override void OnStart() {
            base.OnStart();
            Console.WriteLine("Fragment OnStart");


            if (!_firstRun) StartStepCounterService();

            if (IsBound) return;

            try {
                Console.WriteLine("Starting Step Counter Service...");  
                var serviceIntent = new Intent(Activity, typeof(StepCounterService));
                _serviceConnection = new StepCounterServiceConnection(this);
                Activity.ApplicationContext.BindService(serviceIntent, _serviceConnection, Bind.AutoCreate);
            } catch (Exception e) { }
        }

        private void StartStepCounterService() {
            try {
                Console.WriteLine("Starting Step Counter Service...");
                var service = new Intent(Activity, typeof(StepCounterService));
                Activity.ApplicationContext.StartService(service); 
            } catch (Exception e) { }
        }

        private void UnbindService() {
            Activity.ApplicationContext.UnbindService(_serviceConnection);
            IsBound = false;
        }

        public override void OnDestroy() {
            base.OnDestroy();
            Console.WriteLine("Fragment OnDestroy");
            if (IsBound) {
                UnbindService();
            }
        }

        public override void OnStop() {
            base.OnStop();
            if (IsBound) {
                UnbindService();
            }
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