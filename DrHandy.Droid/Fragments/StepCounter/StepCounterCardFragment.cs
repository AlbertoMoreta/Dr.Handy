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
using DrHandy.DataBase;
using DrHandy.Droid.Custom_Views;
using DrHandy.Droid.Interfaces;
using DrHandy.Droid.Listeners;
using DrHandy.Droid.Services;
using DrHandy.Model;

namespace DrHandy.Droid.Fragments.StepCounter {
    public class StepCounterCardFragment : Fragment, IHealthFragment, IStepDetectedListener {

        public string ShortName { get; set; }
        public bool IsBound { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        private StepCounterServiceConnection _serviceConnection;

        private CustomTextView _steps;

        public StepCounterCardFragment() { }

        public StepCounterCardFragment(string shortName) {
            ShortName = shortName;
        }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            if (savedInstanceState != null) {
                ShortName = savedInstanceState.GetString("ShortName");
            }

        }

        public override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState);
            outState.PutString("ShortName", ShortName);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_stepcounter_card, container, false);

            var moduleName = view.FindViewById<CustomTextView>(Resource.Id.module_name);
            _steps = view.FindViewById<CustomTextView>(Resource.Id.step_count); 
            var stepCounterDate = view.FindViewById<CustomTextView>(Resource.Id.stepcounter_date);

            var module = DBHelper.Instance.GetHealthModuleByShortName(ShortName);
            moduleName.Text = module.Name;

            var stepCounterItems = DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now);
 
            _steps.Text = stepCounterItems.Count > 0 ? stepCounterItems[0].Steps.ToString() : "0";
            stepCounterDate.Text = DateTime.Now.ToString("dd - MMM - yyyy");

            BindService();

            return view;
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

        public void StepDetected() {
            _steps.Text = Binder.GetStepCounterService().Steps.ToString(); 
        }
    }
}