using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS; 
using Android.Support.V7.Widget; 
using Android.Views;
using Android.Widget;
using com.refractored.fab;
using DrHandy.DataBase;
using DrHandy.Droid.Activities;
using DrHandy.Droid.Activities.Sintrom;
using DrHandy.Droid.Adapters;
using DrHandy.Model; 
using Fragment = Android.Support.V4.App.Fragment;
using DrHandy.Droid.Utils;

namespace DrHandy.Droid.Fragments.Sintrom {
    public class SintromTreatmentFragment : Fragment {

        private FloatingActionButton _fab; 
        private LayoutInflater _inflater;
        private ViewGroup _container;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var view = inflater.Inflate(Resource.Layout.fragment_sintrom_treatment, container, false);
            _inflater = inflater;
            _container = container;    

            return view;
        }

        public override void OnResume() {
            base.OnResume();
            RefreshTreatment();
        }

        private void RefreshTreatment()  {
            List<SintromItem> items = GetTreatmentItems();

            var layout = (ViewGroup)View;
            layout.RemoveAllViews();
            var resource = items.Count > 0 ? Resource.Layout.fragment_sintrom_treatment : Resource.Layout.sintrom_empty_treatment;
            var emptyTreatment = _inflater.Inflate(resource, _container, false);
            layout.AddView(emptyTreatment);

            RecyclerView recyclerView = null;
            if (items.Count > 0) {

                recyclerView = View.FindViewById<RecyclerView>(Resource.Id.recycler_view);
                LinearLayoutManager linearLayoutManager = new LinearLayoutManager(Activity);
                linearLayoutManager.Orientation = (int) Orientation.Vertical;
                recyclerView.SetLayoutManager(linearLayoutManager);
                recyclerView.HasFixedSize = true;

                recyclerView.SetAdapter(new SintromTreatmentListAdapter(Activity, items)); 

            }  

            _fab = View.FindViewById<FloatingActionButton>(Resource.Id.fab);
            if(recyclerView != null) { _fab.AttachToRecyclerView(recyclerView); }
            _fab.Click += delegate { 
                var intent = new Intent(Activity, typeof(SintromConfigureTreatment));
                intent.PutExtra("ShortName", ((ModuleDetailActivity) Activity).CurrentHealthModule.ShortName);
                StartActivity(intent);

                Activity.StartActivity(typeof(SintromConfigureTreatment));
            };
        }

        private List<SintromItem> GetTreatmentItems() {
            var userId = HealthModuleUtils.GetCurrentUserId(Activity);
            var sintromINRItems = new List<SintromItem>(DBHelper.Instance.GetSintromINRItemsStartingFromDate(DateTime.Now.AddDays(1), userId));
            var sintromItems = new List<SintromItem>(DBHelper.Instance.GetSintromItemsStartingFromDate(DateTime.Now.AddDays(1), userId));

            return sintromINRItems.Concat(sintromItems).GroupBy(x => x.Date).Select(group => group.First()).OrderBy(x => x.Date).ToList(); 
        }
    }
}