using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS; 
using Android.Support.V7.Widget; 
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using TFG.Droid.Activities.Sintrom;
using TFG.Droid.Adapters;
using TFG.Model; 
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.Sintrom {
    public class SintromTreatmentFragment : Fragment { 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_treatment, container, false);    

            List<SintromItem> items = GetTreatmentItems();

            var _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Activity);
             linearLayoutManager.Orientation = (int) Orientation.Vertical;
             _recyclerView.SetLayoutManager(linearLayoutManager);
            _recyclerView.HasFixedSize = true;

            _recyclerView.SetAdapter(new SintromTreatmentListAdapter(Activity, items));

            var fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.AttachToRecyclerView(_recyclerView);

            fab.Click += delegate { Activity.StartActivity(typeof(SintromConfigureTreatment)); };

            return view;
        }

        private List<SintromItem> GetTreatmentItems() {
            var sintromItems = new List<SintromItem>(DBHelper.Instance.GetSintromItemsStartingFromDate(DateTime.Now.AddDays(1)));
            var l = DBHelper.Instance.GetSintromINRItems();
            return sintromItems.Concat(
                new List<SintromItem>(l)).ToList();
        }
    }
}