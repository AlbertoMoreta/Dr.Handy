using System;
using System.Collections.Generic;
using System.Text;

namespace TFG.DataBase {
    public partial class DBHelper {

		//Sintrom Health Module
        public static readonly string SINTROM_TABLE = "SINTROM";
        public static readonly string INR_TABLE = "INR_VALUES";
        public static readonly string COL_IMAGENAME = "ImageName";
        public static readonly string COL_FRACTION = "Fraction";
        public static readonly string COL_MEDICINE = "Medicine";
        public static readonly string COL_CONTROL = "Control";
        public static readonly string COL_INR = "INR";

		//Sintrom Health Module Queries

        public void CreateSintromTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + SINTROM_TABLE + " (" + COL_DATE + " date primary key, "
                + COL_IMAGENAME + " text, " + COL_FRACTION + " text, " + COL_MEDICINE + " text)";

            Connection.Execute(sql);
        }

        public void CreateINRTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + INR_TABLE + " (" + COL_DATE + " date primary key, "
                + COL_CONTROL + " boolean, " + COL_INR + " integer)";

            Connection.Execute(sql);
        }

        public void InsertSintromItem(SintromTreatmentItem sintromItem) {

            var stringDate = sintromItem.Date.ToString(DATE_FORMAT);

            var sql = "INSERT OR REPLACE INTO " + SINTROM_TABLE + " (" + COL_DATE + ", " + COL_IMAGENAME + ", " + COL_FRACTION + ", " + COL_MEDICINE + ") VALUES "
                + "('" + stringDate + "', '" + sintromItem.ImageName + "', '" + sintromItem.Fraction + "', '" + sintromItem.Medicine + "')";

            Connection.Execute(sql);
        }

        //Get Sintrom Treatment Item from a specific date
        public List<SintromTreatmentItem> GetSintromItemFromDate(DateTime date) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + SINTROM_TABLE + " WHERE " + COL_DATE + " = '" + stringDate + "'";

            return Connection.Query<SintromTreatmentItem>(sql);
        }

        //Get Sintrom Treatment Item from this date onwards
        public List<SintromTreatmentItem> GetSintromItemsStartingFromDate(DateTime date) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + SINTROM_TABLE + " WHERE " + COL_DATE + " >= '" + stringDate + "'";

            return Connection.Query<SintromTreatmentItem>(sql);
        }


        public void InsertSintromINRItem(SintromINRItem sintromInrItem) {
            var stringDate = sintromInrItem.Date.ToString(DATE_FORMAT);

            var sql = "INSERT OR REPLACE INTO " + INR_TABLE + " (" + COL_DATE + ", " + COL_CONTROL + ", " + COL_INR + ") VALUES "
                + "('" + stringDate + "', '" + (sintromInrItem.Control ? "1" : "0")  + "', " + sintromInrItem.INR + ")";

            Connection.Execute(sql);

            var items = GetSintromINRItems();
        }

        public List<SintromINRItem> GetSintromINRItems() {
            var sql = "SELECT * FROM " + INR_TABLE;

            return Connection.Query<SintromINRItem>(sql); 
        }

        public List<SintromINRItem> GetSintromINRItemFromDate(DateTime date) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + INR_TABLE + " WHERE " + COL_DATE + " = '" + stringDate + "'";

            return Connection.Query<SintromINRItem>(sql);
        }

        private void FillSintromTable() {
            DropTable(SINTROM_TABLE);
            DropTable(INR_TABLE);
            CreateSintromTable();
            CreateINRTable();

            var startDate = DateTime.Now.AddMonths(-5);
            var endDate = DateTime.Now.AddMonths(5);

            var rnd = new Random();

            for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))  {
                if (rnd.Next(10) == 1) {
                    //Control day
                    InsertSintromINRItem(new SintromINRItem(day, true, 1.4));
                } else {
                    var medicine = "Sintrom ";
                    var medicineRnd = rnd.Next(3);
                    switch (medicineRnd) {
                        case 0: medicine += "1 mg"; break;
                        case 1: medicine += "2 mg"; break;
                        case 2: medicine += "4 mg"; break;
                    }

                    var imageName = "sintrom_";
                    var fractionRnd = rnd.Next(5);
                    switch (fractionRnd) {
                        case 0: imageName += "1"; break;
                        case 1: imageName += "3_4"; break;
                        case 2: imageName += "1_2"; break;
                        case 3: imageName += "1_4"; break;
                        case 4: imageName += "1_8"; break;
                    }

                    InsertSintromItem(new SintromTreatmentItem(day, imageName, medicine));
                }
            }

        }

    } 
}
