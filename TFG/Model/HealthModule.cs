using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TFG.Logic;

#if __ANDROID__
using Android.Graphics.Drawables;
using Android.Content;
using Android.Content.Res;
using Android.Support.V4.Content;
#endif

namespace TFG.Model {
    public partial class HealthModule  {
        [JsonProperty(Required = Required.Always)]
        public string ShortName { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(CurrentLanguageConverter))]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(CurrentLanguageConverter))]
        public string Description { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public string Color { get; set; } = "red";

        [JsonProperty(PropertyName = "utils-class", Required = Required.Always)] 
        public string UtilsClass { get; set; }

        public Boolean LoginRequired { get; set; } = false;


        public string ConfigFilePath { get; set; }
        public int Position { get; set; }
        public bool Visible { get; set; }
 

    }
}
