using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DrHandy.Droid.Custom_Views;
using DrHandy.Droid.Adapters;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using DrHandy.Droid.Callbacks;
using Android.Support.V7.Widget.Helper;
using Android.Util;
using DrHandy.DataBase;
using DrHandy.Droid.Activities;
using DrHandy.Droid.Listeners;
using DrHandy.Model;
using FloatingActionButton = com.refractored.fab.FloatingActionButton;
using Android.Preferences;

namespace DrHandy.Droid{
	[Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : BaseActivity, HealthCardClickListener {

        private HealthCardAdapter _adapter; 

		protected override void OnCreate (Bundle bundle){
			base.OnCreate (bundle); 

            SetContentView (Resource.Layout.Main); 
            SetUpToolBar(false); 

        }

        private void RefreshLayout() {
            var listLayout = FindViewById<LinearLayout>(Resource.Id.list_layout);
            var emptyLayout = FindViewById<LinearLayout>(Resource.Id.empty_layout);
            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            //DBHelper.Instance.DropTable(DBHelper.TABLE_NAME);
            DBHelper.Instance.Init();
            List<HealthCard> cards = GetCardList();
            if (cards.Count > 0) {
                listLayout.Visibility = ViewStates.Visible;
                emptyLayout.Visibility = ViewStates.Gone;

                var recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);
                recyclerView.SetLayoutManager(new GridLayoutManager(this, 2));

                _adapter = new HealthCardAdapter(this, cards);
                _adapter.SetHealthCardClickListener(this);
                recyclerView.SetAdapter(_adapter);
                ItemTouchHelper.Callback callback = new HealthCardCallback(_adapter);
                ItemTouchHelper helper = new ItemTouchHelper(callback);
                helper.AttachToRecyclerView(recyclerView);
                fab.AttachToRecyclerView(recyclerView);
                var lp = (RelativeLayout.LayoutParams)fab.LayoutParameters;
                lp.RemoveRule(LayoutRules.Below);
                lp.RemoveRule(LayoutRules.CenterHorizontal);
                lp.AddRule(LayoutRules.AlignParentBottom);
                lp.AddRule(LayoutRules.AlignParentRight);
                fab.LayoutParameters = lp;
            } else {
                listLayout.Visibility = ViewStates.Gone;
                emptyLayout.Visibility = ViewStates.Visible;
                var lp = (RelativeLayout.LayoutParams)fab.LayoutParameters;
                lp.RemoveRule(LayoutRules.AlignParentBottom);
                lp.RemoveRule(LayoutRules.AlignParentRight);
                lp.AddRule(LayoutRules.Below, Resource.Id.empty_layout);
                lp.AddRule(LayoutRules.CenterHorizontal);
                fab.LayoutParameters = lp;
            }

            fab.Click += delegate { StartActivity(typeof(ModuleListActivity)); };
        }

        protected override void OnStart() {
            base.OnStart();

            RefreshLayout();

            if (_adapter != null) {
                _adapter.SetCards(GetCardList());
                _adapter.NotifyDataSetChanged();
            }
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
            Intent intent;
            if (healthModule.LoginRequired) { 
                intent = new Intent(this, typeof(SignInActivity));
            } else {
                intent = new Intent(this, typeof(ModuleDetailActivity));
            }

            intent.PutExtra("ShortName", healthModule.ShortName);
            StartActivity(intent);
        }
 
	}
}


