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

namespace TFG.Droid{
	[Activity (Label = "MainActivity", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/AppTheme")]
	public class MainActivity : BaseActivity {
		int count = 1;

		protected override void OnCreate (Bundle bundle){
			base.OnCreate (bundle);
             
			SetContentView (Resource.Layout.Main);

            SetUpToolBar();

            List<HealthCard> cards = new List<HealthCard>();
            cards.Add(new HealthCard(this) { Name = "Card 1" });
            cards.Add(new HealthCard(this) { Name = "Card 2" });
            cards.Add(new HealthCard(this) { Name = "Card 3" });
            cards.Add(new HealthCard(this) { Name = "Card 4" });
            cards.Add(new HealthCard(this) { Name = "Card 5" });

            RecyclerView recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_view);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);
            linearLayoutManager.Orientation = (int) Orientation.Vertical;
            recyclerView.SetLayoutManager(linearLayoutManager);

            HealthCardAdapter adapter = new HealthCardAdapter(cards);
            recyclerView.SetAdapter(adapter);

            ItemTouchHelper.Callback callback = new HealthCardCallback(adapter);
            ItemTouchHelper helper = new ItemTouchHelper(callback);
            helper.AttachToRecyclerView(recyclerView);

        }
	}
}


