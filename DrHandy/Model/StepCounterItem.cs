using System;
using System.Collections.Generic;
using System.Text;

namespace DrHandy.Model {
    public class StepCounterItem {
        public DateTime Date { get; set; }
        public int Steps { get; set; }
        public int Calories { get; set; }
        public double Distance { get; set; }
    }
}
