using System;
using System.Collections.Generic;
using System.Text;
using DrHandy.Logic;
using DrHandy.Model;

namespace DrHandy.DataBase {
    public partial class DBHelper {

        //Step Counter Health Module
        public static readonly string STEPCOUNTER_TABLE = "STEPCOUNTER";
        public static readonly string COL_DATE = "Date";
        public static readonly string COL_STEPS = "Steps";
        public static readonly string COL_CALORIES = "Calories";
        public static readonly string COL_DISTANCE = "Distance";


         //Step Counter Health Module Queries

        public void CreateStepCounterTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + STEPCOUNTER_TABLE + " (" + COL_DATE + " date primary key, "
                + COL_STEPS + " integer, " + COL_CALORIES + " real, " + COL_DISTANCE + " real)";

            Connection.Execute(sql);
        }

        public void UpdateSteps(DateTime date, int steps, int calories, double distance) {

            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "INSERT OR REPLACE INTO " + STEPCOUNTER_TABLE + " (" + COL_DATE + ", " + COL_STEPS + ", " + COL_CALORIES + ", " + COL_DISTANCE + ") VALUES "
                      + "('" + stringDate + "', " + steps + ", " + calories + ", " + distance + ")"; 

            Connection.Execute(sql);
        }

        public List<StepCounterItem> GetStepCounterItemFromDate(DateTime date) {
            var stringDate = date.ToString(DATE_FORMAT);

            var sql = "SELECT * FROM " + STEPCOUNTER_TABLE + " WHERE " + COL_DATE + " = '" + stringDate + "'";

            return Connection.Query<StepCounterItem>(sql);
        }

        public List<StepCounterItem> GetStepCounterItemsFromDateRange(DateTime startDate, DateTime endDate) {
            var sql = "SELECT * " +
                      "FROM " + STEPCOUNTER_TABLE + " " +
                      "WHERE date(" + COL_DATE + ") BETWEEN date('" + startDate.ToString(DATE_FORMAT) + "') " +
                      "AND date('" + endDate.ToString(DATE_FORMAT) + "');";

            return Connection.Query<StepCounterItem>(sql);
        }

        public List<StepCounterItem> GetStepCounterItemsMonthly(string year) {
            var sql = "SELECT " + COL_DATE + ", SUM(" + COL_STEPS +") as " + COL_STEPS + 
                      ", SUM(" + COL_CALORIES + ") as " + COL_CALORIES + ", SUM(" + COL_DISTANCE + ") as " + COL_DISTANCE +
                      " FROM " + STEPCOUNTER_TABLE + " " +
                      "WHERE strftime('%Y', " + COL_DATE + ") = '" + year + "' GROUP BY strftime('%m', " + COL_DATE + ");";

            return Connection.Query<StepCounterItem>(sql);
        }

        private void FillStepCounterTable() {
            DropTable(STEPCOUNTER_TABLE);
            CreateStepCounterTable();

            var startDate = DateTime.Now.AddMonths(-5);
            var endDate = DateTime.Now.AddMonths(5);

            var rnd = new Random();
            var logic = StepCounterLogic.Instance();

            for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1)) {
                var steps = rnd.Next(3000);
                 
                UpdateSteps(day, steps, logic.GetCaloriesFromSteps(steps), logic.GetDistanceFromSteps(steps));

            }  

        }
    }
}
