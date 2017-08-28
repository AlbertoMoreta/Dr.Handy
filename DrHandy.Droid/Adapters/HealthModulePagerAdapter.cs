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
using DrHandy.Droid.Interfaces;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using String = Java.Lang.String;


namespace DrHandy.Droid.Adapters {
    class HealthModulePagerAdapter : FragmentPagerAdapter  {

        private readonly List<Fragment> _fragments;
        private readonly List<string> _titles;

        public HealthModulePagerAdapter(FragmentManager fm) : base(fm) {
            _fragments = new List<Fragment>();
            _titles = new List<string>();
        }

        public override int Count { get { return _fragments.Count; } }

        public override Fragment GetItem(int position) {
            return _fragments.ElementAt(position);
        }

        public override ICharSequence GetPageTitleFormatted(int position){
            return new String(_titles.ElementAt(position));
        } 

        public void AddItem(Fragment f, string title = "") {
            _fragments.Add(f);
            _titles.Add(title);
        }

        public void AddItemAtIndex(Fragment f, int index, string title = "") {
            _fragments.Insert(index, f);
            _titles.Insert(index, title);
        }

        public void RemoveFragment(Fragment f) { 
            _titles.RemoveAt(_fragments.IndexOf(f));
            _fragments.Remove(f);
        }
    }
}
