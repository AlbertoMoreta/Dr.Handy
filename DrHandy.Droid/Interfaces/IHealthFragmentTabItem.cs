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
using DrHandy.Droid.Interfaces;

namespace DrHandy.Droid.Interfaces {
    interface IHealthFragmentTabItem : IHealthFragment{
        string Title { get; }
    }
}