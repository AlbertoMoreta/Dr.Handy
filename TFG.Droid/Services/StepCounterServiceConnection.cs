
using Android.Content;
using Android.OS; 
using TFG.Droid.Listeners;

namespace TFG.Droid.Services {
    /// <summary>
    /// What to do when a client is connected or disconnected from the service
    /// </summary>
    public class StepCounterServiceConnection : Java.Lang.Object, IServiceConnection {

        private IStepDetectedListener _fragment;

        public StepCounterServiceConnection(IStepDetectedListener fragment) {
            _fragment = fragment;
        }

        public void OnServiceConnected(ComponentName name, IBinder service) {
            var serviceBinder = service as StepCounterServiceBinder;
            if (serviceBinder != null) {
                _fragment.Binder = serviceBinder;
                _fragment.IsBound = true;
                serviceBinder.GetStepCounterService().AddListener(_fragment);
            }
        }

        public void OnServiceDisconnected(ComponentName name) {
            _fragment.IsBound = false;
        }
    }
}