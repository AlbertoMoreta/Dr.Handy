using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using DrHandy.Logic;

namespace DrHandy.Model {
    class ColorBlindnessQuestion {

        public int Number { get; set; }
        public string ImageName { get; set; } 
        [JsonConverter(typeof(CurrentLanguageConverter))]
        public string Question { get; set; }
        public int CorrectAnswerPos { get; set; }
        //Position of the answer that should see people with red-green color blindness
        public int RGColorBlindnessPos { get; set; } 
        [JsonConverter(typeof(CurrentLanguageConverter))]
        public string[] Answers { get; set; }
        public string UserAnswer { get; set; }
          
    
    }
}
