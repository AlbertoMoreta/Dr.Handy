
using DrHandy.Droid.Services;

namespace DrHandy.Droid.Listeners {
    public interface IStepDetectedListener {
        bool IsBound { get; set; }
        StepCounterServiceBinder Binder { get; set; }
        void StepDetected();
    }
}