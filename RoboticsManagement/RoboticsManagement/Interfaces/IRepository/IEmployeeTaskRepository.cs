using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IEmployeeTaskRepository
    {
        public EmployeeTask GetTaskById(int taskId);
        public List<EmployeeTask> GetUnDoneTasks();
        public List<EmployeeTask> GetDoneTasks();
    }
}
