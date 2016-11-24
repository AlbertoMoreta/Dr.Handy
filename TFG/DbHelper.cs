
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
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


        //Step Counter Module
        public static readonly string STEPCOUNTER_TABLE = "STEPCOUNTER";
        public static readonly string COL_DATE = "Date";
        public static readonly string COL_STEPS = "Steps";
        public static readonly string COL_CALORIES = "Calories";
        public static readonly string COL_DISTANCE = "Distance";
        public static readonly string DATE_FORMAT = "yy-MM-dd";


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

        public void InitHealthModule(HealthModuleType module) {
            switch (module) {
                case HealthModuleType.ColorBlindnessTest: break;
                case HealthModuleType.StepCounter: CreateStepCounterTable(); break;
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


        public void CreateStepCounterTable() {
            var sql = "CREATE TABLE IF NOT EXISTS " + STEPCOUNTER_TABLE + " (" + COL_DATE + " text primary key, "
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
    }


}
