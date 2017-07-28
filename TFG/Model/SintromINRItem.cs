using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {
    public class SintromINRItem : SintromItem {
        public string UserId { get; set; }
        public DateTime Date { get; set; } 
        public bool Control { get; set; }
        public double INR { get; set; }

        public SintromINRItem() { } 
        public SintromINRItem(string userId, DateTime date, bool control, double inr) {
            UserId = userId;
            Date = date;
            Control = control;
            INR = inr;
        }
    }
}
