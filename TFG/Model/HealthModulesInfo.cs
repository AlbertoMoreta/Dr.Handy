using System;
using System.Collections.Generic;
using System.Text;
using TFG.Logic;

namespace TFG.Model {

    public enum HealthModuleType {
        ColorBlindnessTest, StepCounter, Sintrom
    }

    public static class HealthModulesInfo {

        public static List<HealthModuleType> GetHealthModules { get; private set; } = new List<HealthModuleType> {
            HealthModuleType.ColorBlindnessTest,
            HealthModuleType.StepCounter,
            HealthModuleType.Sintrom
        };

        public static int HealthModuleId(this HealthModuleType module) {
            return (int) module;
        }

        public static HealthModuleType GetHealthModuleTypeById(int id) {
            return (HealthModuleType) id;
        }

        public static string HealthModuleName(this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: return GetStringFromResourceName("color_blindness_test_name");
                case HealthModuleType.StepCounter: return GetStringFromResourceName("step_counter_name");
                case HealthModuleType.Sintrom: return GetStringFromResourceName("sintrom_name");
                default: return "Error";
            }
            
        }

        public static string HealthModuleDescription (this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: return "Module1 Description";
                case HealthModuleType.StepCounter: return "Module2 Description";
                case HealthModuleType.Sintrom: return "Module3 Description";
                default: return "Error";
            }
        }

        public static string HealthModuleColor(this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: return "purple";
                case HealthModuleType.StepCounter: return "blue";
                case HealthModuleType.Sintrom: return "orange";
                default: return "purple";
            }

        }

        public static string GetHealthModuleColorFromHealthModuleName(string moduleName){
            if (HealthModuleType.ColorBlindnessTest.HealthModuleName().Equals(moduleName)) {
                return HealthModuleColor(HealthModuleType.ColorBlindnessTest);
            }
            if (HealthModuleType.StepCounter.HealthModuleName().Equals(moduleName)) {
                return HealthModuleColor(HealthModuleType.StepCounter);
            }
            if (HealthModuleType.Sintrom.HealthModuleName().Equals(moduleName)) {
                return HealthModuleColor(HealthModuleType.Sintrom);
            } 
            return null;
        }

        public static string GetStringFromResourceName(string resName){

#if __ANDROID__
            var context = Android.App.Application.Context;
            return context.GetString(context.Resources.GetIdentifier(resName, "string", context.PackageName));
#elif __IOS__

#endif
            return null;
        }

        public static NotificationItem GetNotificationItem(this HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: return null;
                case HealthModuleType.StepCounter: return null;
                case HealthModuleType.Sintrom: return SintromLogic.Instance().GetNotificationitem(); 
                default: return null;
            }
        }
    }
}
