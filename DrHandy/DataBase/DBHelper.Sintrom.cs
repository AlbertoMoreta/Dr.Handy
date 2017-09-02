using System;
using System.Collections.Generic; 
using DrHandy.Model;

namespace DrHandy.DataBase {
    public partial class DBHelper {

		//Sintrom Health Module
        public static readonly string SINTROM_TABLE = "SINTROM";
        public static readonly string INR_TABLE = "INR_VALUES";
        public static readonly string COL_USERID = "UserId";
        public static readonly string COL_IMAGENAME = "ImageName";
        public static readonly string COL_FRACTION = "Fraction";
        public static readonly string COL_MEDICINE = "Medicine";
        public static readonly string COL_CONTROL = "Control";
        public static readonly string COL_INR = "INR";

		//Sintrom Health Module Queries

        public void CreateSintromTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + SINTROM_TABLE + " (" + COL_USERID + " text, " + COL_DATE + " date, "
                + COL_IMAGENAME + " text, " + COL_FRACTION + " text, " + COL_MEDICINE + " text, primary key (" + COL_USERID + ", " + COL_DATE + "))";

            Connection.Execute(sql);
        }

        public void CreateINRTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + INR_TABLE + " (" + COL_USERID + " text, " + COL_DATE + " date, "
                + COL_CONTROL + " boolean, " + COL_INR + " real, primary key (" + COL_USERID + ", " + COL_DATE + "))";

            Connection.Execute(sql);
        }

        public void InsertSintromItem(SintromTreatmentItem sintromItem) {

            var stringDate = sintromItem.Date.ToString(DATE_FORMAT);

            var sql = "INSERT OR REPLACE INTO " + SINTROM_TABLE + " (" + COL_USERID + ", " + COL_DATE + ", " + COL_IMAGENAME + ", " + COL_FRACTION + ", " + COL_MEDICINE + ") VALUES "
                + "('" + sintromItem.UserId + "', '" + stringDate + "', '" + sintromItem.ImageName + "', '" + sintromItem.Fraction + "', '" + sintromItem.Medicine + "')";

            Connection.Execute(sql);
        }

        public void UpdateSintromItem(SintromTreatmentItem sintromItem) {
            if (sintromItem.ImageName.Equals("")) {
                if (SintromItemExistsForDate(sintromItem.Date, sintromItem.UserId)) {
                    RemoveSintromItemFromDate(sintromItem.Date, sintromItem.UserId);
                }
            } else {
                InsertSintromItem(sintromItem);
            }
            
        }

        public bool SintromItemExistsForDate(DateTime date, string userId) {
            return GetSintromItemFromDate(date, userId).Count > 0;

        }

        //Get Sintrom Treatment Item from a specific date
        public List<SintromTreatmentItem> GetSintromItemFromDate(DateTime date, string userId) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + SINTROM_TABLE + " WHERE " + COL_USERID + " = '" + userId + "' AND " + COL_DATE + " = '" + stringDate + "'";

            return Connection.Query<SintromTreatmentItem>(sql);
        }

        public void RemoveSintromItemFromDate(DateTime date, string userId) {
			var stringDate = date.ToString(DATE_FORMAT);

            var sql = "DELETE FROM " + SINTROM_TABLE + " WHERE " + COL_USERID + " = '" + userId + "' AND " + COL_DATE + " = '" + stringDate + "'";

			Connection.Execute(sql);
        }

        //Get Sintrom Treatment Item from this date onwards
        public List<SintromTreatmentItem> GetSintromItemsStartingFromDate(DateTime date, string userId) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + SINTROM_TABLE + " WHERE " + COL_USERID + " = '" + userId + "' AND " + COL_DATE + " >= '" + stringDate + "'";

            return Connection.Query<SintromTreatmentItem>(sql);
        }


        public void InsertSintromINRItem(SintromINRItem sintromInrItem) {
            var stringDate = sintromInrItem.Date.ToString(DATE_FORMAT);

            var sql = "INSERT OR REPLACE INTO " + INR_TABLE + " (" + COL_USERID + ", " +COL_DATE + ", " + COL_CONTROL + ", " + COL_INR + ") VALUES "
                + "('" + sintromInrItem.UserId + "', '" + stringDate + "', '" + (sintromInrItem.Control ? "1" : "0")  + "', " + sintromInrItem.INR + ")";

            Connection.Execute(sql);
        }

        public void RemoveINRItemFromDate(DateTime date, string userId) {
			var stringDate = date.ToString(DATE_FORMAT);

            var sql = "DELETE FROM " + INR_TABLE + " WHERE " + COL_USERID + " = '" + userId + "' AND " + COL_DATE + " = '" + stringDate + "'";

			Connection.Execute(sql);
        }


        public void RemoveOrHideSintromINRItem(DateTime date, string userId) {
            var items = GetSintromINRItemFromDate(date, userId);
            if (items != null && items.Count > 0) {
                var item = items[0];
                if (item.INR == 0) {
                    RemoveINRItemFromDate(date, userId);
                } else {
					var stringDate = date.ToString(DATE_FORMAT);
                    var sql = "UPDATE " + INR_TABLE + " SET " + COL_CONTROL + "= '0' WHERE " + COL_USERID + " = '" + userId + "' AND " + COL_DATE + " = '" + stringDate + "'";
					Connection.Execute(sql);
                }
                
            }
        }
		

        public List<SintromINRItem> GetSintromINRItems() {
            var sql = "SELECT * FROM " + INR_TABLE;

            return Connection.Query<SintromINRItem>(sql); 
        }

        public List<SintromINRItem> GetSintromINRItemFromDate(DateTime date, string userId) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + INR_TABLE + " WHERE " + COL_USERID + " = '" + userId + "' AND " + COL_DATE + " = '" + stringDate + "'";

            return Connection.Query<SintromINRItem>(sql);
        }

		//Get Sintrom INR Item from this date onwards
        public List<SintromINRItem> GetSintromINRItemsStartingFromDate(DateTime date, string userId) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + INR_TABLE + " WHERE " + COL_USERID + " = '" + userId + "' AND " + COL_DATE + " >= '" + stringDate + "'";

            return Connection.Query<SintromINRItem>(sql);
        }

        private void FillSintromTable(string userId) {
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
                    var inrValue = Math.Round(rnd.NextDouble() * (4.0 - 1.0) + 1.0, 2);
                    InsertSintromINRItem(new SintromINRItem(userId, day, true, inrValue));
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

                    InsertSintromItem(new SintromTreatmentItem(userId, day, imageName, medicine));
                }
            }

        }

    } 
}
