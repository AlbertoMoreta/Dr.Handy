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

namespace DrHandy.Droid.Services {
    public class StepCounterServiceBinder : Binder{
        private StepCounterService _service;

        public StepCounterServiceBinder(StepCounterService service) {
            _service = service; 
        }

        public StepCounterService GetStepCounterService() {
            return _service;
        }

    }
}