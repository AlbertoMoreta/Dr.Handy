using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TFG.Droid.Custom_Views;
using TFG.Droid.Adapters;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using TFG.Droid.Callbacks;
using Android.Support.V7.Widget.Helper;
using com.refractored.fab;
using TFG.Model;

namespace TFG.Droid{
	[Activity (Label = "MainActivity", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/AppTheme")]
	public class MainActivity : BaseActivity {

        private HealthCardAdapter _adapter;

		protected override void OnCreate (Bundle bundle){
			base.OnCreate (bundle);
             
			SetContentView (Resource.Layout.Main);

            SetUpToolBar();


            //DBHelper.Instance.DropTable(DBHelper.TABLE_NAME);
            DBHelper.Instance.Init(); 

            List<HealthCard> cards = GetCardList();

            RecyclerView recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);
            linearLayoutManager.Orientation = (int) Orientation.Vertical;
            recyclerView.SetLayoutManager(linearLayoutManager);

            _adapter = new HealthCardAdapter(cards);
            recyclerView.SetAdapter(_adapter);

            ItemTouchHelper.Callback callback = new HealthCardCallback(_adapter);
            ItemTouchHelper helper = new ItemTouchHelper(callback);
            helper.AttachToRecyclerView(recyclerView);

            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.AttachToRecyclerView(recyclerView);

            fab.Click += delegate { StartActivity(typeof(ModuleListActivity)); };

        }

        protected override void OnStart() {
            base.OnStart();
            _adapter.SetCards(GetCardList());
            _adapter.NotifyDataSetChanged();
        }

        private List<HealthCard> GetCardList() {
            List<HealthCard> cards = new List<HealthCard>();

            List<HealthModule> modules = DBHelper.Instance.GetModules();
            foreach(HealthModule module in modules) {
                cards.Add(new HealthCard(this) { Name = module.Name });
            }

            return cards;
        }
	}
}


