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
using TFG.Droid.Fragments.StepCounter;
using TFG.Droid.Interfaces;
using TFG.Model;

namespace TFG.Droid{
    public static class HealthModulesInfoExtension {

        public static IHealthFragment GetHeaderFragmentFromHealthModuleName(string moduleName) {
            if (HealthModuleType.ColorBlindnessTest.HealthModuleName().Equals(moduleName)) { return new CBTHeaderFragment(); }
            if (HealthModuleType.StepCounter.HealthModuleName().Equals(moduleName)) { return new StepCounterHeaderFragment(); }
            if (HealthModuleType.Module3.HealthModuleName().Equals(moduleName)) { return null; }
            return null;
        }

        public static IHealthFragment GetBodyFragmentFromHealthModuleName(string moduleName) {
            if (HealthModuleType.ColorBlindnessTest.HealthModuleName().Equals(moduleName)) { return new CBTBodyFragment(); } 
            if (HealthModuleType.StepCounter.HealthModuleName().Equals(moduleName)) { return new StepCounterBodyFragment(); } 
            if (HealthModuleType.Module3.HealthModuleName().Equals(moduleName)) { return null; }
            return null;
        }

        public static int GetStyleFromHealthModuleName(string moduleName) {
            var context = Application.Context;
            if (HealthModuleType.ColorBlindnessTest.HealthModuleName().Equals(moduleName)) {
                return context.Resources.GetIdentifier("AppTheme_Purple", "style", context.PackageName);
            }
            if (HealthModuleType.StepCounter.HealthModuleName().Equals(moduleName)) {
                return context.Resources.GetIdentifier("AppTheme_Orange", "style", context.PackageName);
            }
            if (HealthModuleType.Module3.HealthModuleName().Equals(moduleName)) { return -1; }
            return -1;
        }

    }
}
