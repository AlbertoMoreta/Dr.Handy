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
using TFG.Model;

namespace TFG.Droid{
    public static class HealthModulesInfoExtension {

        public static Fragment GetHeadFragmentFromHealthModule(this HealthModules module) {
            switch (module) {
                case HealthModules.Module1: return null;
                case HealthModules.Module2: return null;
                case HealthModules.Module3: return null;
                default: return null;
            }
        }

        public static Fragment GetBodyFragmentFromHealthModule(this HealthModules module) {
            switch (module) {
                case HealthModules.Module1: return null;
                case HealthModules.Module2: return null;
                case HealthModules.Module3: return null;
                default: return null;
            }
        }

    }
}