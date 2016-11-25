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
using TFG.Droid.Listeners;
using TFG.Logic;

namespace TFG.Droid.Services {
    /// <summary>
    /// Service for the step counter sensor
    /// </summary>
    [Service(Enabled = true)]
    public class StepCounterService : Service, ISensorEventListener  {

        public int Steps { get; set; }
        public int Calories { get; set; }
        public double Distance { get; set; }
        public DateTime DateLastStep { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        private bool _isRunning;
        private StepDetectedListener _listener;

        private StepCounterLogic _logic;

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

            if (!Utils.StepCounterUtils.IsKitKatWithStepCounter(PackageManager)) {
                Console.WriteLine("The device is not compatible with the step sensor");
                StopSelf();
                return;
            }

            _logic = StepCounterLogic.Instance();

            if (!_isRunning) {
                var sensorManager = (SensorManager) GetSystemService(SensorService);
                var sensor = sensorManager.GetDefaultSensor(SensorType.StepDetector);
                sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
            }

            _isRunning = true;
            var items = DBHelper.Instance.GetStepCounterItemFromDate(DateTime.Now);
            if (items.Count > 0) {
                var item = items.ElementAt(0);
                DateLastStep = DateTime.ParseExact(item.Date,
                    DBHelper.DATE_FORMAT,
                    System.Globalization.CultureInfo.InvariantCulture);

                Steps = item.Steps;
                Calories = item.Calories;
                Distance = item.Distance;
            } else {
                Steps = 0;
                Calories = 0;
                Distance = 0;
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
                Calories = 0;
                Distance = 0;
                DateLastStep = CurrentDate;
            } else {
                Steps++;
                Calories = _logic.GetCaloriesFromSteps(Steps);
                Distance = _logic.GetDistanceFromSteps(Steps);
            }

            DBHelper.Instance.UpdateSteps(DateTime.Now, Steps, Calories, Distance);

            _listener.StepDetected();
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

        public void SetListener(StepDetectedListener listener) {
            _listener = listener;
        }
    }
}