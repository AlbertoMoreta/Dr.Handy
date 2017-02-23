 
 
using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using TFG.Droid.Adapters;
using TFG.Droid.Custom_Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.Sintrom {
    public class SintromCalendarFragment : Fragment {

        public DateTime Date { get; set; }

        public SintromCalendarFragment() { }

        public SintromCalendarFragment(DateTime date) {
            Date = date;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.sintrom_calendar, container, false);

            if (savedInstanceState != null)  { 
                Date = new DateTime(savedInstanceState.GetLong("date"));
            }

            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);
            recyclerView.SetLayoutManager(new GridLayoutManager(Activity, 7));

            var adapter = new SintromCalendarAdapter(Activity, Date); 
            recyclerView.SetAdapter(adapter);

            return view;
        }

        public override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState); 
            outState.PutLong("date", Date.Ticks);
        }
    }
}