
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TFG.Logic;
using TFG.Model;

namespace TFG.DataBase {
    public partial class DBHelper {
         
        public static readonly string DB_NAME = "HealthApp.db3";

        public static readonly string TABLE_NAME = "HEALTH_MODULE";
        private static readonly string COL_KEY_SHORTNAME = "Shortname";
        private static readonly string COL_CONFIG_FILE_PATH = "ConfigFilePath";
        private static readonly string COL_POSITION = "Position";
        private static readonly string COL_VISIBLE = "Visible";
        private static readonly string COL_DATE = "Date";
        public static readonly string DATE_FORMAT = "yyyy-MM-dd";
         

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
            var sql = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (" + COL_KEY_SHORTNAME + " text primary key, "
                + COL_CONFIG_FILE_PATH + " text," + COL_POSITION + " int," + COL_VISIBLE + " boolean)";

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
        
        public void AddHealthModule(HealthModule module) {
            var sql = "INSERT INTO " + TABLE_NAME + " (" + COL_KEY_SHORTNAME + ", " + COL_CONFIG_FILE_PATH + ", " + COL_POSITION + ", " + COL_VISIBLE + ") VALUES " 
                + "('" + module.ShortName + "', '" + module.ConfigFilePath + "', " + Count() + ", 1)" ;

            Connection.Execute(sql);

         //   InitHealthModule(module);
        }

        public void DeleteHealthModule(HealthModule module) {
            var sql = "DELETE FROM " + TABLE_NAME + " WHERE " + COL_KEY_SHORTNAME + " = '" + module.ShortName + "'";

            Connection.Execute(sql);
        }

        public bool CheckIfExists(HealthModule module) {
            try {
                var sql = "SELECT COUNT(*) FROM " + TABLE_NAME + " WHERE " + COL_KEY_SHORTNAME + " = '" + module.ShortName + "'";
                var count = Connection.ExecuteScalar<int>(sql);
                return count > 0;
            } catch(SQLite.SQLiteException e) {
                return false;
            }

        }

        public bool CheckIfVisible(HealthModule module) {
            try {
                var sql = "SELECT COUNT(*) FROM " + TABLE_NAME + " WHERE " + COL_KEY_SHORTNAME + " = '" + module.ShortName 
                    + "' AND " + COL_VISIBLE + " = 1";
                var count = Connection.ExecuteScalar<int>(sql);
                return count > 0;
            } catch (SQLite.SQLiteException e) {
                return false;
            }
        }

        public void ChangeModuleVisibility(HealthModule module, bool value) {
            var sql = "UPDATE " + TABLE_NAME + " SET " + COL_VISIBLE + " = " + (value ? 1 : 0) 
                + " WHERE " + COL_KEY_SHORTNAME + " = '" + module.ShortName + "'";

            Connection.Execute(sql);
        }

        public void ChangeModulePosition(HealthModule module, int position) {
            var sql = "UPDATE " + TABLE_NAME + " SET " + COL_POSITION + " = " + position
                + " WHERE " + COL_KEY_SHORTNAME + " = '" + module.ShortName + "'";

            Connection.Execute(sql);
        }

        public List<HealthModule> GetModules() {
            var sql = "SELECT * FROM " + TABLE_NAME 
                + " WHERE " + COL_VISIBLE + " = 1 ORDER BY " + COL_POSITION + " ASC";
            var healthModules = Connection.Query<HealthModule>(sql);
            foreach (HealthModule hm in healthModules) {
                HealthModulesConfigReader.PopulateHealthModule(hm); 
            }
            return healthModules;

        }

        public HealthModule GetHealthModuleByShortName(string shortName) {
            var sql = "SELECT * FROM " + TABLE_NAME
                + " WHERE " + COL_KEY_SHORTNAME + " = '" + shortName + "' AND " + COL_VISIBLE + " = 1 LIMIT 1";
            var healthModule = Connection.Query<HealthModule>(sql).ElementAt(0);
            HealthModulesConfigReader.PopulateHealthModule(healthModule);
            return healthModule;
        }

        public void DropTable(string tableName) {
            var sql = "DROP TABLE IF EXISTS " + tableName;
            Connection.Execute(sql);
        } 
         
    } 
}
