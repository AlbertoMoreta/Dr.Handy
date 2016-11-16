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
using TFG.Droid.Interfaces;
using TFG.Droid.Services;

namespace TFG.Droid.Fragments.StepCounter {
    class StepCounterHeaderFragment : Fragment, IHealthFragment {

        public bool IsBound { get; set; }
        public StepCounterServiceBinder Binder { get; set; }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            StartStepCounterService();
        
        }

        private void StartStepCounterService() {
            try {
                var serviceIntent = new Intent(Activity, typeof(StepCounterService));
                var serviceConnection = new StepCounterServiceConnection(this);
                Activity.ApplicationContext.BindService(serviceIntent, serviceConnection, Bind.AutoCreate);
            }catch(Exception e) { }
        }
    }
}