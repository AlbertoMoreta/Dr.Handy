using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace TFG.Logic {
    class ColorBlindnessLogic  {

        public static readonly int TOTAL_QUESTIONS = 4;
        public static readonly int ANSWERS_COUNT = 4;

        public int CurrentQuestion { get; set; } = 0;
        public List<Model.ColorBlindnessQuestion> Questions { get; set; }

        private static ColorBlindnessLogic _instance;

        public static ColorBlindnessLogic Instance() {
            if (_instance == null)  {
                _instance = new ColorBlindnessLogic();
            }

            return _instance;
        }
  

#if __ANDROID__
        private static Android.Content.Context _context;

        public static ColorBlindnessLogic Instance(Android.Content.Context context){
            _context = context;
            if (_instance == null) {
                _instance = new ColorBlindnessLogic();
            }
            return _instance;
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
