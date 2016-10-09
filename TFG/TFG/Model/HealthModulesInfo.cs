using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {

    public enum HealthModules {
        Module1, Module2, Module3
    }

    public static class HealthModulesInfo {

        public static List<HealthModules> GetHealthModules { get; private set; } = new List<HealthModules> {
            HealthModules.Module1,
            HealthModules.Module2,
            HealthModules.Module3
        };

        public static string HealthModuleName(this HealthModules module) {
            switch (module) {
                case HealthModules.Module1: return "Module1";
                case HealthModules.Module2: return "Module2";
                case HealthModules.Module3: return "Module3";
                default: return "Error";
            }
            
        }

        public static string HealthModuleDescription (this HealthModules module) {
            switch (module) {
                case HealthModules.Module1: return "Module1 Description";
                case HealthModules.Module2: return "Module2 Description";
                case HealthModules.Module3: return "Module3 Description";
                default: return "Error";
            }
        } 
    }
}
