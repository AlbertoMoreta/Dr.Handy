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
    public class StepCounterService : Service, ISensorEventListener  {

        public int Steps { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        private bool _isRunning;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) {
            Console.WriteLine("Service OnStartCommand");

            Init();

            return StartCommandResult.Sticky;
        } 

        public override IBinder OnBind(Intent intent) {
            Binder = new StepCounterServiceBinder(this);
            
            return Binder;
        }

        private void Init() {
            if (!_isRunning) {
                var sensorManager = (SensorManager) GetSystemService(SensorService);
                var sensor = sensorManager.GetDefaultSensor(SensorType.StepDetector);
                sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
            }

            _isRunning = true;
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) {
        }

        public void OnSensorChanged(SensorEvent e) {
            Steps++;
            Console.WriteLine(Steps + " Steps");
        }

        public override void OnDestroy() {
            base.OnDestroy();

            try {
                var sensorManager = (SensorManager) GetSystemService(SensorService);
                sensorManager.UnregisterListener(this); 
                _isRunning = false;
            } catch (Exception e) { 
            } 
        }
    }
}