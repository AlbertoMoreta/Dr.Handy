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
using Android.Content.PM;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using TFG.Droid.Callbacks;
using Android.Support.V7.Widget.Helper;
using Android.Util;
using TFG.DataBase;
using TFG.Droid.Activities;
using TFG.Droid.Listeners;
using TFG.Model;
using FloatingActionButton = com.refractored.fab.FloatingActionButton;

namespace TFG.Droid{
	[Activity (Label = "MainActivity", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : BaseActivity, HealthCardClickListener {

        private HealthCardAdapter _adapter;
	    private RecyclerView _recyclerView;

		protected override void OnCreate (Bundle bundle){
			base.OnCreate (bundle); 

            SetContentView (Resource.Layout.Main);
            SetUpToolBar(false);

            //DBHelper.Instance.DropTable(DBHelper.TABLE_NAME);
            DBHelper.Instance.Init(); 

            List<HealthCard> cards = GetCardList();

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view); 
            _recyclerView.SetLayoutManager(new GridLayoutManager(this, 2));


            _adapter = new HealthCardAdapter(this, cards);
            _adapter.SetHealthCardClickListener(this);
            _recyclerView.SetAdapter(_adapter);

            ItemTouchHelper.Callback callback = new HealthCardCallback(_adapter);
            ItemTouchHelper helper = new ItemTouchHelper(callback);
            helper.AttachToRecyclerView(_recyclerView);

            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.AttachToRecyclerView(_recyclerView);

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
                cards.Add(new HealthCard(this, module) {
                    Name = module.Name,
                    Icon = module.GetIcon(this)
                });
            }
            return cards;
        }

	    public void OnHealthCardClick(HealthModule healthModule)  {
            if (healthModule.LoginRequired) {
                var intent = new Intent(this, typeof(SignInActivity));
                intent.PutExtra("ShortName", healthModule.ShortName);
                StartActivity(intent);
            } else {
                var intent = new Intent(this, typeof(ModuleDetailActivity)); 
	            intent.PutExtra("ShortName", healthModule.ShortName);
	            StartActivity(intent); 
            }
        }
 
	}
}


