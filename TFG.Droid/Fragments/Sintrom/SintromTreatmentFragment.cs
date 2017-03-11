using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS; 
using Android.Support.V7.Widget; 
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using TFG.DataBase;
using TFG.Droid.Activities;
using TFG.Droid.Activities.Sintrom;
using TFG.Droid.Adapters;
using TFG.Model; 
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.Sintrom {
    public class SintromTreatmentFragment : Fragment { 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_treatment, container, false);    

            FloatingActionButton fab;
            RecyclerView recyclerView = null;

            List<SintromItem> items = GetTreatmentItems(); 

            if (items.Count > 0) {

                recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);
                LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Activity);
                linearLayoutManager.Orientation = (int) Orientation.Vertical;
                recyclerView.SetLayoutManager(linearLayoutManager);
                recyclerView.HasFixedSize = true;

                recyclerView.SetAdapter(new SintromTreatmentListAdapter(Activity, items)); 

            } else {
                //Replace layout with empty treatment layout
                var layout = (ViewGroup) view;
                layout.RemoveAllViews();
                var emptyTreatment = inflater.Inflate(Resource.Layout.sintrom_empty_treatment, container, false);
                layout.AddView(emptyTreatment);
            }  
            
            fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            if(recyclerView != null) { fab.AttachToRecyclerView(recyclerView); }
            fab.Click += delegate { Activity.StartActivity(typeof(SintromConfigureTreatment)); };

            return view;
        }

        private List<SintromItem> GetTreatmentItems() {
            var sintromINRItems = new List<SintromItem>(DBHelper.Instance.GetSintromINRItemsStartingFromDate(DateTime.Now.AddDays(1)));
            var sintromItems = new List<SintromItem>(DBHelper.Instance.GetSintromItemsStartingFromDate(DateTime.Now.AddDays(1)));

            return sintromINRItems.Concat(sintromItems).GroupBy(x => x.Date).Select(group => group.First()).OrderBy(x => x.Date).ToList(); 
        }
    }
}