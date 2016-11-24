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
using Java.Sql;
using Java.Text;

namespace TFG.Droid.Services {
    /// <summary>
    /// Service for the step counter sensor
    /// </summary>
    [Service(Enabled = true)]
    public class StepCounterService : Service, ISensorEventListener  {

        public int Steps { get; set; }
        public DateTime DateLastStep { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        private bool _isRunning;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) {

            Init();
#if DEBUG
            Console.WriteLine("Service started");
#endif

            return StartCommandResult.Sticky;
        } 

        public override IBinder OnBind(Intent intent) {
            Binder = new StepCounterServiceBinder(this);
            
            return Binder;
        }

        private void Init() {

           // DBHelper.Instance.DropTable("STEPCOUNTER");
            if (!_isRunning) {
                DBHelper.Instance.CreateStepCounterTable();
                var sensorManager = (SensorManager) GetSystemService(SensorService);
                var sensor = sensorManager.GetDefaultSensor(SensorType.StepDetector);
                sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
            }

            _isRunning = true;
            var s = DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now);
            if (s.Count > 0) {
                Steps = s.ElementAt(0).Steps;
                DateLastStep = DateTime.ParseExact(s.ElementAt(0).Date,
                    DBHelper.DATE_FORMAT,
                    System.Globalization.CultureInfo.InvariantCulture);
            } else {
                Steps = 0;
            }
            
            Console.WriteLine("Initial Steps = " + Steps);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) {
        }

        public void OnSensorChanged(SensorEvent e) {
#if DEBUG 
            Console.WriteLine("Step detected");
#endif
            var CurrentDate = DateTime.Now;
            if (!DateLastStep.Date.Equals(CurrentDate.Date)) {
                Steps = 0;
                DateLastStep = CurrentDate;
            } else {
                Steps++;
            }

            DBHelper.Instance.UpdateSteps(DateTime.Now, Steps);
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