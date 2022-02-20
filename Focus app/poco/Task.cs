using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_app.poco
{
    class Task
    {
        public Task(String TaskID,String TaskName,String TaskStatus,String CreatedAt)
        {
            this.TaskName = TaskName;
            this.TaskStatus = TaskStatus;
            this.CreatedAt = CreatedAt;
            this.TaskID = TaskID;
        }
        public string TaskName { set; get; }
        public string TaskStatus { set; get; }
        public string CreatedAt { set; get; }
        public string TaskID { set; get; }
    }
}
