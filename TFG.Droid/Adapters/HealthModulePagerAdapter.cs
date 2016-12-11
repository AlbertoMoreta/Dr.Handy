using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using TFG.Droid.Interfaces;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using String = Java.Lang.String;


namespace TFG.Droid.Adapters {
    class HealthModulePagerAdapter : FragmentPagerAdapter  {

        private readonly List<Fragment> _fragments;

        public HealthModulePagerAdapter(FragmentManager fm) : base(fm) {
            _fragments = new List<Fragment>();
        }

        public override int Count { get { return _fragments.Count; } }

        public override Fragment GetItem(int position) {
            return _fragments.ElementAt(position);
        }

        public override ICharSequence GetPageTitleFormatted(int position){
            return new String(((IHealthFragmentTabItem) _fragments.ElementAt(position)).Title);
        }

        public void AddItem(Fragment f) {
            _fragments.Add(f);
        }

        public void RemoveFragment(Fragment f) { 
            _fragments.Remove(f); 
        }
    }
}