using Focus_app.poco;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace Focus_app
{

    class DB
    {
        private static SQLiteConnection getDBConnection()
        {
            bool isDBInit = !File.Exists(Constants.connectionFileName);
            if (Constants.connection == null)
            {
                Constants.connection = new SQLiteConnection("Data Source="+Constants.connectionFileName+";Version=3");
                Constants.connection.Open();
                if (isDBInit)
                {
                    Constants.db.putIntoDB(Constants.scratchDDL);
                    Console.WriteLine("isDBInit");
                }
            }
            return Constants.connection;
        }

        public void putIntoDB(String query)
        {
            Console.WriteLine(query);
            try
            {
                new SQLiteCommand(query, getDBConnection()).ExecuteNonQuery();

            }
            catch (Exception ee) { Console.WriteLine(ee.StackTrace); }
        }
        public SQLiteDataReader getFromDB(String query)
        {
            try
            {
                Console.WriteLine(query);
                SQLiteCommand cmd = new SQLiteCommand(query, getDBConnection());
                return cmd.ExecuteReader();
            }
            catch (Exception ee) { Console.WriteLine(ee.StackTrace); }
            return null;
        }



        public void addTask(String taskName)
        {
            putIntoDB("insert into task (task_name,status) values ('" +taskName + "','New')");
        }
        public List<Task> getAllTasks()
        {
            List<Task> tasks = new List<Task>();
            SQLiteDataReader tasksReader = getFromDB("select * from task where is_done=0");
            if (tasksReader.HasRows)
            {
                while (tasksReader.Read())
                {
                    Task task = new Task(tasksReader["task_id"].ToString(),tasksReader["task_name"].ToString(), tasksReader["status"].ToString(), tasksReader["created_at"].ToString());
                    tasks.Add(task);
                }
            }
            return tasks;
        }
        public Task getCurrentTask()
        {
            SQLiteDataReader tasksReader = getFromDB("select * from task where is_current=1");
            if (tasksReader.HasRows)
            {
                tasksReader.Read();
                return new Task(tasksReader["task_id"].ToString(), tasksReader["task_name"].ToString(), tasksReader["status"].ToString(), tasksReader["created_at"].ToString());  
            }
            return null;
        }
        public void updateCurrentTask(Task task)
        {
            putIntoDB("update task set is_current=0");
            putIntoDB("update task set is_current=1 where task_id="+task.TaskID);

        }
        public void updateTaskDone(Task task)
        {
            putIntoDB("update task set is_done=1 where task_id=" + task.TaskID);
        }
    }
}
