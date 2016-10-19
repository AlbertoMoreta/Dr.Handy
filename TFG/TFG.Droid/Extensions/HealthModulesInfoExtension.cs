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

        public static Fragment GetHeadFragmentFromHealthModule(this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.Module1: return null;
                case HealthModuleType.Module2: return null;
                case HealthModuleType.Module3: return null;
                default: return null;
            }
        }

        public static Fragment GetBodyFragmentFromHealthModule(this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.Module1: return null;
                case HealthModuleType.Module2: return null;
                case HealthModuleType.Module3: return null;
                default: return null;
            }
        }

    }
}