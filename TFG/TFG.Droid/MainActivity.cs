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

            HealthCardAdapter adapter = new HealthCardAdapter(this);
            adapter.SetCards(cards);
            DynamicListView listView = FindViewById<DynamicListView>(Resource.Id.listview);

            listView.SetViewList(cards);
            listView.Adapter = adapter;
            listView.ChoiceMode = ChoiceMode.Single;

        }
	}
}


