 
 
using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using DrHandy.Droid.Adapters;
using DrHandy.Droid.Custom_Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace DrHandy.Droid.Fragments.Sintrom {
    public class SintromCalendarFragment : Fragment {
         
        public DateTime Date { get; set; }
        private SintromCalendarAdapter _adapter;

        public SintromCalendarFragment() { }

        public SintromCalendarFragment(DateTime date) {
            Date = date; 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.sintrom_calendar, container, false);

            if (savedInstanceState != null)  { 
                Date = new DateTime(savedInstanceState.GetLong("date"));
            }
            _adapter = new SintromCalendarAdapter(Activity, Date); 

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);
            recyclerView.SetLayoutManager(new GridLayoutManager(Activity, 7));

            recyclerView.SetAdapter(_adapter);

            return view;
        }

        public override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState); 
            outState.PutLong("date", Date.Ticks); 
        }

        public void SetPreviousMonth() {
            Date = Date.AddMonths(-1);
            _adapter.Date = Date;
            _adapter.NotifyDataSetChanged();
        }

        public void SetNextMonth() {
            Date = Date.AddMonths(1);
            _adapter.Date = Date;
            _adapter.NotifyDataSetChanged();
        }
    }
}