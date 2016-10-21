using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {
    class ColorBlindnessQuestion {

        public int Number { get; set; }
        public string ImageName { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Answers { get; set; }
        public string UserAnswer { get; set; }
          
    
    }
}
