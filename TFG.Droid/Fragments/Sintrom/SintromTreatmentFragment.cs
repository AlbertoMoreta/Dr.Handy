using System;
using System.Collections.Generic; 
using Android.OS; 
using Android.Support.V7.Widget; 
using Android.Views;
using Android.Widget;
using com.refractored.fab; 
using TFG.Droid.Custom_Views;
using Fragment = Android.Support.V4.App.Fragment;

namespace TFG.Droid.Fragments.Sintrom {
    public class SintromTreatmentFragment : Fragment { 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_treatment, container, false);    

            //List<HealthCard> cards = GetTreatment();

            var _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Activity);
             linearLayoutManager.Orientation = (int) Orientation.Vertical;
             _recyclerView.SetLayoutManager(linearLayoutManager);
            _recyclerView.HasFixedSize = true;

            //_recyclerView.SetAdapter(new HealthCardAdapter(cards));

            var fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.AttachToRecyclerView(_recyclerView);

            fab.Click += delegate { /*StartActivity(typeof(ModuleListActivity));*/ };

            return view;
        }

        /*private List<HealthCard> GetTreatment() {
            List<HealthCard> cards = new List<HealthCard>();

            List<HealthModule> modules = DBHelper.Instance.GetModules();
            foreach (HealthModule module in modules) {
                cards.Add(new HealthCard(this, module) { Name = module.Name });
            }

            return cards;
        }*/
    }
}