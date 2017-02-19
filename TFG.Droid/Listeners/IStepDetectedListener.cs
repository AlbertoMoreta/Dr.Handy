
using TFG.Droid.Services;

namespace TFG.Droid.Listeners {
    public interface IStepDetectedListener {
        bool IsBound { get; set; }
        StepCounterServiceBinder Binder { get; set; }
        void StepDetected();
    }
}