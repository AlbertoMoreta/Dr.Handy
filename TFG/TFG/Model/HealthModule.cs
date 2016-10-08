using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {

    public enum HealthModule {
        Module1, Module2, Module3
    }

    public static class HealthModules {

        public static List<HealthModule> GetHealthModules { get; private set; } = new List<HealthModule> {
            HealthModule.Module1,
            HealthModule.Module2,
            HealthModule.Module3
        };

        public static string HealthModuleName(this HealthModule module) {
            switch (module) {
                case HealthModule.Module1: return "Module1";
                case HealthModule.Module2: return "Module2";
                case HealthModule.Module3: return "Module3";
                default: return "Error";
            }
            
        }

        public static string HealthModuleDescription (this HealthModule module) {
            switch (module) {
                case HealthModule.Module1: return "Module1 Description";
                case HealthModule.Module2: return "Module2 Description";
                case HealthModule.Module3: return "Module3 Description";
                default: return "Error";
            }
        } 
    }
}
