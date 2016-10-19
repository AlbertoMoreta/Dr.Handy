using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {

    public enum HealthModuleType {
        Module1, Module2, Module3
    }

    public static class HealthModulesInfo {

        public static List<HealthModuleType> GetHealthModules { get; private set; } = new List<HealthModuleType> {
            HealthModuleType.Module1,
            HealthModuleType.Module2,
            HealthModuleType.Module3
        };

        public static string HealthModuleName(this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.Module1: return GetStringFromResourceName("color_blindness_test_name");
                case HealthModuleType.Module2: return "Module2";
                case HealthModuleType.Module3: return "Module3";
                default: return "Error";
            }
            
        }

        public static string HealthModuleDescription (this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.Module1: return "Module1 Description";
                case HealthModuleType.Module2: return "Module2 Description";
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

        }
    }
}
