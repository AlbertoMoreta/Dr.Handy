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
using TFG.Droid.Fragments.StepCounter;
using TFG.Droid.Interfaces;

namespace TFG.Droid.Services {
    class StepCounterServiceConnection : Java.Lang.Object, IServiceConnection {

        private StepCounterHeaderFragment _fragment;

        public StepCounterServiceConnection(StepCounterHeaderFragment fragment) {
            _fragment = fragment;
        }

        public void OnServiceConnected(ComponentName name, IBinder service) {
            var serviceBinder = service as StepCounterServiceBinder;
            if (serviceBinder != null)
            {
                _fragment.Binder = serviceBinder;
                _fragment.IsBound = true;
            }
        }

        public void OnServiceDisconnected(ComponentName name) {
            _fragment.IsBound = false;
        }
    }
}