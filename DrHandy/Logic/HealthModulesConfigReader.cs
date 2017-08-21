using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using TFG.Model;
using HealthModule = TFG.Model.HealthModule;

namespace TFG.Logic {
    public static class HealthModulesConfigReader {

        private static readonly string CONFIG_FILES_ROOT = "ConfigFiles";
         

        public static List<HealthModule> GetHealthModules() {

            List<HealthModule> healthModules = new List<HealthModule>(); 
             
#if __IOS__
           // jsonFile = File.ReadAllText(fileName);
#elif __ANDROID__

            //Read Android Health Modules Config Files
            var context = Android.App.Application.Context; 
            var files = context.Assets.List(CONFIG_FILES_ROOT);
            foreach(string file in files) { 
                var filePath = CONFIG_FILES_ROOT + "/" + file; 
                var healthModule = ParseHealthModuleFromPath(filePath);
                healthModule.ConfigFilePath = filePath; 
                healthModules.Add(healthModule);
            } 

#endif 

            return healthModules; 

        }

        public static HealthModule ParseHealthModuleFromPath(string path) { 
            var context = Android.App.Application.Context;
            StreamReader stream = new StreamReader(context.Assets.Open(path));
            var jsonFile = stream.ReadToEnd();
            stream.Close();
            return JsonConvert.DeserializeObject<HealthModule>(jsonFile);
        }


        public static void PopulateHealthModule(HealthModule healthModule) {
            var context = Android.App.Application.Context;
            StreamReader stream = new StreamReader(context.Assets.Open(healthModule.ConfigFilePath));
            var jsonFile = stream.ReadToEnd();
            stream.Close();
            JsonConvert.PopulateObject(jsonFile, healthModule);
        }


    }
}
