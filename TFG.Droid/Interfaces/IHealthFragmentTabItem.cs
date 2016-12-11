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
using TFG.Droid.Interfaces;

namespace TFG.Droid.Interfaces {
    interface IHealthFragmentTabItem : IHealthFragment{
        string Title { get; }
    }
}