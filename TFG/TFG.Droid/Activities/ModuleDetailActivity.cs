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

namespace TFG.Droid.Activities {
    [Activity(Label = "ModuleDetailActivity", Theme = "@style/AppTheme")]
    public class ModuleDetailActivity : BaseActivity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.module_detail);

            SetUpToolBar();

            FragmentManager fragmentManager = FragmentManager;
            FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

            CBTHeaderFragment headerFragment = new CBTHeaderFragment();
            fragmentTransaction.Add(Resource.Id.fragments_container, headerFragment);

            CBTBodyFragment bodyFragment = new CBTBodyFragment();
            fragmentTransaction.Add(Resource.Id.fragments_container, bodyFragment);

            fragmentTransaction.Commit();

        }
    }
}