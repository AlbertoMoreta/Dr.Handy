using System;
using System.Collections.Generic; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Java.Util;

namespace DrHandy.Logic {

    /*
     * CurrentLanguageConverter - This class uses the current device lenguage for selecting the 
     * right language from a JSON file (follow this link to see an example of a JSON file with 
     * multilanguage support -> https://github.com/AlbertoMoreta/Dr.Handy/wiki/Adding-a-new-health-module-for-Android#adding-the-config-file
     */
    class CurrentLanguageConverter : JsonConverter {
        //English as default language
        private static readonly string DEFAULT_LANGUAGE = "English";

        public override bool CanConvert(Type objectType) {
            return (objectType == typeof(List<string>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            JToken token = JToken.Load(reader);
            var defaultValue = ""; 
            if (token.Type == JTokenType.Array) { 
                var currentLanguage = Locale.Default.GetDisplayLanguage(Locale.Default);  
                foreach (var objs in token.Children<JObject>()) { 
                    foreach (var pair in objs) {
                        var loc = new Locale(pair.Key);
                        Console.WriteLine(pair.Key);
                        var language = loc.GetDisplayLanguage(loc);
                        if (language.Equals(currentLanguage)) {
                            return pair.Value.ToObject<string>();
                        }else if (language.Equals(DEFAULT_LANGUAGE)) {
                            defaultValue = pair.Value.ToString();
                        }
                        
                    }

                }
                return defaultValue;
            }
            return "error";
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {}
    }
}
