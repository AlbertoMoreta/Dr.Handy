using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {

    public enum HealthModuleType {
        ColorBlindnessTest, StepCounter, Module3
    }

    public static class HealthModulesInfo {

        public static List<HealthModuleType> GetHealthModules { get; private set; } = new List<HealthModuleType> {
            HealthModuleType.ColorBlindnessTest,
            HealthModuleType.StepCounter,
            HealthModuleType.Module3
        };

        public static string HealthModuleName(this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: return GetStringFromResourceName("color_blindness_test_name");
                case HealthModuleType.StepCounter: return GetStringFromResourceName("step_counter_name");
                case HealthModuleType.Module3: return "Module3";
                default: return "Error";
            }
            
        }

        public static string HealthModuleDescription (this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: return "Module1 Description";
                case HealthModuleType.StepCounter: return "Module2 Description";
                case HealthModuleType.Module3: return "Module3 Description";
                default: return "Error";
            }
        }

        public static string GetStringFromResourceName(string resName){

#if __ANDROID__
            var context = Android.App.Application.Context;
            return context.GetString(context.Resources.GetIdentifier(resName, "string", context.PackageName));
#elif __IOS__

#endif
            return null;
        }
    }
}
