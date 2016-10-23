using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {
    class ColorBlindnessQuestion {

        public int Number { get; set; }
        public string ImageName { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        //Answer that should see people with red-green color blindness
        public string RGColorBlindness { get; set; }
        //Answer that should see people with total color blindness
        public string TotalColorBlindness { get; set; }
        public List<string> Answers { get; set; }
        public string UserAnswer { get; set; }
          
    
    }
}
