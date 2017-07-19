using System; 
using System.Globalization; 

using Android.App; 
using Android.Content.PM; 
using Android.OS; 
using Android.Support.V4.View;
using TFG.DataBase;
using TFG.Droid.Adapters;
using TFG.Droid.Custom_Views;
using TFG.Droid.Fragments.Sintrom;
using TFG.Model; 

namespace TFG.Droid.Activities.Sintrom {
    
    [Activity(Label = "SintromConfigureTreatment", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Landscape)]
    public class SintromConfigureTreatment : BaseActivity {

        private readonly int CENTER_POS = 1;
        private HealthModulePagerAdapter _adapter;
        private ViewPager _pager;
        private CustomTextView _currentDate;

        public SintromCalendarFragment CurrentMonth { get; private set; }
        public SintromCalendarFragment PreviousMonth { get; private set; }
        public SintromCalendarFragment NextMonth { get; private set; }
         
        protected override void OnCreate(Bundle savedInstanceState)  {
            base.OnCreate(savedInstanceState); 

            var shortName = Intent.GetStringExtra("ShortName"); 
            var module = DBHelper.Instance.GetHealthModuleByShortName(shortName);

            var moduleName = module.Name;
            var theme = module.GetTheme(this);
            if (theme != -1) { SetTheme(theme);}

            Window.DecorView.Background = module.GetBackground(this); 
  

            SetContentView (Resource.Layout.sintrom_configure_treatment);

            _currentDate = FindViewById<CustomTextView>(Resource.Id.current_date);
            _currentDate.Text = DateTime.Now.ToString("MMMM yyyy");

            _pager = FindViewById<ViewPager>(Resource.Id.pager);
            _pager.PageSelected += ScrollChange;
            _adapter = new HealthModulePagerAdapter(SupportFragmentManager);  

            _adapter.AddItem(PreviousMonth = new SintromCalendarFragment(DateTime.Now.AddMonths(-1)));
            _adapter.AddItem(CurrentMonth = new SintromCalendarFragment(DateTime.Now));
            _adapter.AddItem(NextMonth = new SintromCalendarFragment(DateTime.Now.AddMonths(1))); 
            _adapter.NotifyDataSetChanged();

            _pager.Adapter = _adapter;
            _pager.SetCurrentItem(CENTER_POS, false); 

            InitDays();
        }

        private void InitDays() {
            
            var days = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;

            //Reorganize days based on current culture 
            string[] currentCultureDays = new string[7];
            for (var i = 0; i < days.Length; i++) {
                currentCultureDays[i] = days[((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek + i) % days.Length];
            }

            for (var i = 0; i < currentCultureDays.Length; i++) {
                FindViewById<CustomTextView>(Resources.GetIdentifier("day" + i, "id", PackageName)).Text = currentCultureDays[i];
            } 


        }

        private void ScrollChange(object sender, ViewPager.PageSelectedEventArgs e) {
            if (CENTER_POS > e.Position) {
                //Left Scroll
                PreviousMonth.SetPreviousMonth();
                CurrentMonth.SetPreviousMonth();
                NextMonth.SetPreviousMonth();  
            } else if(CENTER_POS < e.Position) {
                //Right Scroll
                PreviousMonth.SetNextMonth();
                CurrentMonth.SetNextMonth();
                NextMonth.SetNextMonth(); 
            }
            _adapter.NotifyDataSetChanged();
           
            _pager.SetCurrentItem(CENTER_POS, false);

            
             _currentDate.Text = CurrentMonth.Date.ToString("MMMM yyyy");
        }
    }
}