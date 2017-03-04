using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {
    public class SintromINRItem {

        public DateTime Date { get; set; } 
        public bool Control { get; set; }
        public double INR { get; set; }

        public SintromINRItem() { } 
        public SintromINRItem(DateTime date, bool control, double inr) {
            Date = date;
            Control = control;
            INR = inr;
        }
    }
}
