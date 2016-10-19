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

        public static Fragment GetHeaderFragmentFromHealthModuleName(string moduleName) {
            if (HealthModuleType.Module1.HealthModuleName().Equals(moduleName)) { return null; }
            if (HealthModuleType.Module2.HealthModuleName().Equals(moduleName)) { return null; }
            if (HealthModuleType.Module3.HealthModuleName().Equals(moduleName)) { return null; }
            return null;
        }

        public static Fragment GetBodyFragmentFromHealthModule(string moduleName) {
            if (HealthModuleType.Module1.HealthModuleName().Equals(moduleName)) { return null; } 
            if (HealthModuleType.Module2.HealthModuleName().Equals(moduleName)) { return null; } 
            if (HealthModuleType.Module3.HealthModuleName().Equals(moduleName)) { return null; }
            return null;
        }

    }
}