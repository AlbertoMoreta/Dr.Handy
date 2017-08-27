using System; 
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS; 
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views; 
using com.refractored;
using DrHandy.Droid.Adapters; 
using DrHandy.Droid.Interfaces;
using DrHandy.Droid.Listeners;
using DrHandy.Droid.Services;
using DrHandy.Droid.Utils;

namespace DrHandy.Droid.Fragments.StepCounter {
    /// <summary>
    /// Body fragment for the Step Counter health module
    /// </summary>
    class StepCounterBodyFragment : Fragment, IHealthFragment, IStepDetectedListener { 

        public bool IsBound { get; set; }
        public StepCounterServiceBinder Binder { get; set; }
        private StepCounterServiceConnection _serviceConnection;

        private StepCounterChartFragment _weeklyResultsFragment;
        private StepCounterChartFragment _yearlyResultsFragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_tabs, container, false);

            var pager = view.FindViewById<ViewPager>(Resource.Id.pager); 
            var adapter = new HealthModulePagerAdapter(((AppCompatActivity) Activity).SupportFragmentManager);
            adapter.AddItem(new StepCounterQuickResultsFragment(), "Quick Results");

            var weeklyResultsTitle = Activity.GetString(Activity.Resources.GetIdentifier("weekly_results",
                "string", Activity.PackageName));
            _weeklyResultsFragment = new StepCounterChartFragment(ChartUtils.VisualizationMetric.Weekly);
            adapter.AddItem(_weeklyResultsFragment, weeklyResultsTitle);

            var yearlyResultsTitle = Activity.GetString(Activity.Resources.GetIdentifier("yearly_results",
                "string", Activity.PackageName));
            _yearlyResultsFragment = new StepCounterChartFragment(ChartUtils.VisualizationMetric.Yearly);
            adapter.AddItem(_yearlyResultsFragment, yearlyResultsTitle);

            pager.Adapter = adapter;

            var tabs = view.FindViewById<PagerSlidingTabStrip>(Resource.Id.tabs);
            tabs.SetViewPager(pager);
            tabs.TabTextColor = ColorStateList.ValueOf(Color.AntiqueWhite);
            tabs.TabTextColorSelected = ColorStateList.ValueOf(Color.White);
            tabs.IndicatorColor = Color.White;

            BindService();

            return view;
        }

        private void BindService() {
            try {
#if DEBUG
                Console.WriteLine("Binding client to the service...");
#endif
                var serviceIntent = new Intent(Activity, typeof(StepCounterService));
                _serviceConnection = new StepCounterServiceConnection(this);
                Activity.ApplicationContext.BindService(serviceIntent, _serviceConnection, Bind.AutoCreate);
            } catch (Exception e) { }
        }

        private void UnbindService() {
#if DEBUG
            Console.WriteLine("Unbinding client from the service...");
#endif
            Activity.ApplicationContext.UnbindService(_serviceConnection);
            IsBound = false;
        }

        public override void OnDestroy() {
            base.OnDestroy();
            if (IsBound) {
                Binder.GetStepCounterService().RemoveListener(this);
                UnbindService();
            }
        }

        public override void OnStop() {
            base.OnStop();
            if (IsBound) {
                Binder.GetStepCounterService().RemoveListener(this);
                UnbindService();
            }
        }

        public void StepDetected() {
            _weeklyResultsFragment.RefreshCharts();
            _yearlyResultsFragment.RefreshCharts();
        }
    }
}