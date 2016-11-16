using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TFG.Droid.Services {
    [Service(Enabled = true)]
    public class StepCounterService : Service, ISensorEventListener{
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            throw new NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            throw new NotImplementedException();
        }
    }
}