using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace TFG.Logic {
    class ColorBlindnessLogic {

        public List<Model.ColorBlindnessQuestion> Questions { get; set; }

#if __ANDROID__
        private Android.Content.Context _context;

        public ColorBlindnessLogic(Android.Content.Context context){
            _context = context;
        }

#endif


        public List<Model.ColorBlindnessQuestion> GetQuestions()  {

            if (Questions == null) {
                string jsonFile;
                string fileName = "JSON/ColorBlindnessTest.json";
#if __IOS__
            jsonFile = File.ReadAllText(fileName);
#elif __ANDROID__
                StreamReader stream = new StreamReader(_context.Assets.Open(fileName));
                jsonFile = stream.ReadToEnd();
                stream.Close();

#endif
                Questions = JsonConvert.DeserializeObject<List<Model.ColorBlindnessQuestion>>(jsonFile);
            }

            return Questions;;
        }

    }
}
