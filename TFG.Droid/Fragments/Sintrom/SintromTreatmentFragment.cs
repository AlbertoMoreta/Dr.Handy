using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
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

        private FloatingActionButton _fab;
        private RecyclerView _recyclerView;
        private LayoutInflater _inflater;
        private ViewGroup _container;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_treatment, container, false);
            _inflater = inflater;
            _container = container;  

            FloatingActionButton fab;
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recycler_view); 
            
            
            

            return view;
        }

        public override void OnStart() {
            base.OnStart();
            RefreshTreatment();
        }

        private void RefreshTreatment()  {
            List<SintromItem> items = GetTreatmentItems(); 

            if (items.Count > 0) { 
                LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Activity);
                linearLayoutManager.Orientation = (int) Orientation.Vertical;
                _recyclerView.SetLayoutManager(linearLayoutManager);
                _recyclerView.HasFixedSize = true;

                _recyclerView.SetAdapter(new SintromTreatmentListAdapter(Activity, items)); 

            } else {
                //Replace layout with empty treatment layout
                var layout = (ViewGroup) View;
                layout.RemoveAllViews();
                var emptyTreatment = _inflater.Inflate(Resource.Layout.sintrom_empty_treatment, _container, false);
                layout.AddView(emptyTreatment);
            }  

            _fab = View.FindViewById<FloatingActionButton>(Resource.Id.fab);
            if(_recyclerView != null) { _fab.AttachToRecyclerView(_recyclerView); }
            _fab.Click += delegate { 
                var intent = new Intent(Activity, typeof(SintromConfigureTreatment));
                intent.PutExtra("ShortName", ((ModuleDetailActivity) Activity).CurrentHealthModule.ShortName);
                StartActivity(intent);

                Activity.StartActivity(typeof(SintromConfigureTreatment));
            };
        }

        private List<SintromItem> GetTreatmentItems() {
            var sintromINRItems = new List<SintromItem>(DBHelper.Instance.GetSintromINRItemsStartingFromDate(DateTime.Now.AddDays(1)));
            var sintromItems = new List<SintromItem>(DBHelper.Instance.GetSintromItemsStartingFromDate(DateTime.Now.AddDays(1)));

            return sintromINRItems.Concat(sintromItems).GroupBy(x => x.Date).Select(group => group.First()).OrderBy(x => x.Date).ToList(); 
        }
    }
}