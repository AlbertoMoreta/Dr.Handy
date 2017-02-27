
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using TFG.Logic;
using TFG.Model;

namespace TFG {
    class DBHelper {
         
        public static readonly string DB_NAME = "HealthApp.db3";

        public static readonly string TABLE_NAME = "HEALTH_MODULE";
        private static readonly string COL_KEY_ID = "Id";
        private static readonly string COL_NAME = "Name";
        private static readonly string COL_DESCRIPTION = "Description";
        private static readonly string COL_POSITION = "Position";
        private static readonly string COL_VISIBLE = "Visible";
        public static readonly string DATE_FORMAT = "yyyy-MM-dd";


        //Step Counter Health Module
        public static readonly string STEPCOUNTER_TABLE = "STEPCOUNTER";
        public static readonly string COL_DATE = "Date";
        public static readonly string COL_STEPS = "Steps";
        public static readonly string COL_CALORIES = "Calories";
        public static readonly string COL_DISTANCE = "Distance";

        //Sintrom Health Module
        public static readonly string SINTROM_TABLE = "SINTROM";
        public static readonly string COL_IMAGENAME = "ImageName";
        public static readonly string COL_FRACTION = "Fraction";
        public static readonly string COL_MEDICINE = "Medicine";
        public static readonly string COL_CONTROL = "Control";  


        private static DBHelper _instance;

        public static DBHelper Instance {
            get {
                if(_instance == null) {
                    _instance = new DBHelper();
                }
                return _instance;
            }
        }

        private static SQLiteConnection _connection;
        public static SQLiteConnection Connection {
            get {
                if(_connection == null) {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal); 
#if __IOS__
                    string libraryPath = Path.Combine(path, "..", "Library");
                    var dbPath = Path.Combine(libraryPath, DB_NAME);
#elif __ANDROID__
                    var dbPath = Path.Combine(path, DB_NAME);
#endif 
                    _connection = new SQLiteConnection(dbPath);
                }
                return _connection;
            }
        }


        public void Init() {
            CreateHealthModulesTable();
        }


        private void CreateHealthModulesTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (" + COL_KEY_ID + " integer primary key autoincrement, "
                + COL_NAME + " text," + COL_DESCRIPTION + " text," + COL_POSITION + " int," + COL_VISIBLE + " boolean)";

            Connection.Execute(sql);
        }

        public int Count() {
            try {
                var sql = "SELECT COUNT(*) FROM " + TABLE_NAME;
                var count = Connection.ExecuteScalar<int>(sql);
                return count;
            } catch (SQLite.SQLiteException e) {
                return -1;
            }
        }
        
        public void AddHealthModule(HealthModuleType module) {
            var sql = "INSERT INTO " + TABLE_NAME + " (" + COL_NAME + ", " + COL_DESCRIPTION + ", " + COL_POSITION + ", " + COL_VISIBLE + ") VALUES " 
                + "('" + module.HealthModuleName() + "', '" + module.HealthModuleDescription() + "', " + Count() + ", 1)" ;

            Connection.Execute(sql);

            InitHealthModule(module);
        }

        public void DeleteHealthModule(HealthModuleType module) {
            var sql = "DELETE FROM " + TABLE_NAME + " WHERE " + COL_NAME + " = '" + module.HealthModuleName() + "'";

            Connection.Execute(sql);
        }

        public void InitHealthModule(HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: break;
                case HealthModuleType.StepCounter: CreateStepCounterTable(); break;
                case HealthModuleType.Sintrom: CreateSintromTable(); break;
            }
        }

        public bool CheckIfExists(HealthModuleType module) {
            try {
                var sql = "SELECT COUNT(*) FROM " + TABLE_NAME + " WHERE " + COL_NAME + " = '" + module.HealthModuleName() + "'";
                var count = Connection.ExecuteScalar<int>(sql);
                return count > 0;
            } catch(SQLite.SQLiteException e) {
                return false;
            }

        }

        public bool CheckIfVisible(HealthModuleType module) {
            try {
                var sql = "SELECT COUNT(*) FROM " + TABLE_NAME + " WHERE " + COL_NAME + " = '" + module.HealthModuleName() 
                    + "' AND " + COL_VISIBLE + " = 1";
                var count = Connection.ExecuteScalar<int>(sql);
                return count > 0;
            } catch (SQLite.SQLiteException e) {
                return false;
            }
        }

        public void ChangeModuleVisibility(HealthModuleType module, bool value) {
            var sql = "UPDATE " + TABLE_NAME + " SET " + COL_VISIBLE + " = " + (value ? 1 : 0) 
                + " WHERE " + COL_NAME + " = '" + module.HealthModuleName() + "'";

            Connection.Execute(sql);
        }

        public void ChangeModulePosition(HealthModule module, int position) {
            var sql = "UPDATE " + TABLE_NAME + " SET " + COL_POSITION + " = " + position
                + " WHERE " + COL_NAME + " = '" + module.Name + "'";

            Connection.Execute(sql);
        }

        public List<HealthModule> GetModules() {
            var sql = "SELECT * FROM " + TABLE_NAME 
                + " WHERE " + COL_VISIBLE + " = 1 ORDER BY " + COL_POSITION + " ASC";
            return Connection.Query<HealthModule>(sql);

        } 

        public void DropTable(string tableName) {
            var sql = "DROP TABLE IF EXISTS " + tableName;
            Connection.Execute(sql);
        }



        //Step Counter Health Module Queries

        public void CreateStepCounterTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + STEPCOUNTER_TABLE + " (" + COL_DATE + " date primary key, "
                + COL_STEPS + " integer, " + COL_CALORIES + " integer, " + COL_DISTANCE + " integer)";

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



        //Sintrom Health Module Queries

        public void CreateSintromTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + SINTROM_TABLE + " (" + COL_DATE + " date primary key, "
                + COL_IMAGENAME + " text, " + COL_FRACTION + " text, " + COL_MEDICINE + " text, " + COL_CONTROL  + " boolean)";

            Connection.Execute(sql);
        }

        public void InsertSintromItem(SintromTreatmentItem sintromItem) {

            var stringDate = sintromItem.Date.ToString(DATE_FORMAT);

            var sql = "INSERT OR REPLACE INTO " + SINTROM_TABLE + " (" + COL_DATE + ", " + COL_IMAGENAME + ", " + COL_FRACTION + ", " + COL_MEDICINE + ", " + COL_CONTROL + ") VALUES "
                + "('" + stringDate + "', '" + sintromItem.ImageName + "', '" + sintromItem.Fraction + "', '" + sintromItem.Medicine + "', " + (sintromItem.Control ? "1" : "0") + ")";

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

        private void FillSintromTable() {
            DropTable(SINTROM_TABLE);
            CreateSintromTable();

            var startDate = DateTime.Now.AddMonths(-5);
            var endDate = DateTime.Now.AddMonths(5);

            var rnd = new Random();

            for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))  {
                if (rnd.Next(10) == 1) {
                    //Control day
                    InsertSintromItem(new SintromTreatmentItem(day, "", "", true));
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

                    InsertSintromItem(new SintromTreatmentItem(day, imageName, medicine, false));
                }
            }

        }

    }


}
