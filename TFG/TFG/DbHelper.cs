using Android.App;
using Android.Database.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TFG.Droid.Custom_Views;
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
        
        
        public void AddHealthModule(HealthModules module) {
            var sql = "INSERT INTO " + TABLE_NAME + " (" + COL_NAME + ", " + COL_DESCRIPTION + ", " + COL_POSITION + ", " + COL_VISIBLE + ") VALUES " 
                + "('" + module.HealthModuleName() + "', '" + module.HealthModuleDescription() + "', " + Count() + ", 1)" ;

            Connection.Execute(sql);
        } 

        public bool CheckIfExists(HealthModules module) {
            try {
                var sql = "SELECT COUNT(*) FROM " + TABLE_NAME + " WHERE " + COL_NAME + " = '" + module.HealthModuleName() + "'";
                var count = Connection.ExecuteScalar<int>(sql);
                return count > 0;
            } catch(SQLite.SQLiteException e) {
                return false;
            }

        }

        public bool CheckIfVisible(HealthModules module) {
            try {
                var sql = "SELECT COUNT(*) FROM " + TABLE_NAME + " WHERE " + COL_NAME + " = '" + module.HealthModuleName() 
                    + "' AND " + COL_VISIBLE + " = 1";
                var count = Connection.ExecuteScalar<int>(sql);
                return count > 0;
            } catch (SQLite.SQLiteException e) {
                return false;
            }
        }

        public void ChangeModuleVisibility(HealthModules module, bool value) {
            var sql = "UPDATE " + TABLE_NAME + " SET " + COL_VISIBLE + " = " + (value ? 1 : 0) 
                + " WHERE " + COL_NAME + " = '" + module.HealthModuleName() + "'";

            Connection.Execute(sql);
        }

        public List<HealthModule> GetModules() {
            var sql = "SELECT " +  COL_NAME+" FROM " + TABLE_NAME 
                + " WHERE " + COL_VISIBLE + " = 1 ORDER BY " + COL_POSITION + " ASC";
            return Connection.Query<HealthModule>(sql);

        } 


        public void DropTable(string tableName) {
            var sql = "DROP TABLE IF EXISTS " + tableName;
            Connection.Execute(sql);
        }

    }


}
