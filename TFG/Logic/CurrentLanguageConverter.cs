using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Java.Util;

namespace TFG.Logic {
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
                        var language = loc.GetDisplayLanguage(loc);
                        if (language.Equals(currentLanguage)) {
                            if (pair.Value.Type == JTokenType.String)  {
                                return pair.Value.ToString();
                            }
                            if (pair.Value.Type == JTokenType.Array) { 
                                return pair.Value.ToObject<string[]>();
                            }
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
