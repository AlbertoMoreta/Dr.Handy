using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.Model {
    public class SintromTreatmentItem { 

        public DateTime Date { get; set; }
        private string _imageName;
        public string ImageName {
            get {return _imageName;}
            set { _imageName = value; ImageNameToFraction(); }
        } 
        public string Fraction { get; private set; }
        public string Medicine { get; set; }

        public SintromTreatmentItem() { }

        public SintromTreatmentItem(DateTime date, string imageName, string medicine) {
            Date = date;
            ImageName = imageName;
            Medicine = medicine;
        }

        private void ImageNameToFraction() {
            switch (_imageName) {
                case "sintrom_1":   Fraction = "1"; break;
                case "sintrom_1_2": Fraction = "1/2"; break;
                case "sintrom_1_4": Fraction = "1/4"; break;
                case "sintrom_1_8": Fraction = "1/8"; break;
                case "sintrom_3_4": Fraction = "3/4"; break;
            }
        }
    }
}
