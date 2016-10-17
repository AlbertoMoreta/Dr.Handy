using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TFG.Droid.Fragments.ColorBlindnessTest;
using TFG.Droid.Interfaces;

namespace TFG.Droid.Activities {
    [Activity(Label = "ModuleDetailActivity", Theme = "@style/AppTheme")]
    public class ModuleDetailActivity : BaseActivity {

        public IHealthFragment HeaderFragment { get; private set; }
        public IHealthFragment BodyFragment { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.module_detail);

            SetUpToolBar();

            FragmentManager fragmentManager = FragmentManager;
            FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

            HeaderFragment = new CBTHeaderFragment();
            fragmentTransaction.Add(Resource.Id.fragments_container, HeaderFragment as Fragment);

            BodyFragment = new CBTBodyFragment();
            fragmentTransaction.Add(Resource.Id.fragments_container, BodyFragment as Fragment);

            fragmentTransaction.Commit();

        }
    }
}