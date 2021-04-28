using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface ITaskForEmployeeRepository
    {
        public List<TaskForEmployee> GetTaskIdByEmployeeId(string employeeId);
        public List<EmployeeTask> GetTasksForEmployee(string employeeId);
    }
}
