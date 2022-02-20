using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_app
{
    class Constants
    {
        public static DB db = new DB();
        public static SQLiteConnection connection;
        public static String connectionFileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/focus.sqlite";
        public static String scratchDDL = "CREATE TABLE task (task_id INTEGER  PRIMARY KEY AUTOINCREMENT,task_name TEXT,status TEXT,created_at DATETIME NOT NULL DEFAULT(datetime('now')),is_current BOOLEAN  DEFAULT(0),is_done BOOLEAN  DEFAULT(0));";
    }
}
