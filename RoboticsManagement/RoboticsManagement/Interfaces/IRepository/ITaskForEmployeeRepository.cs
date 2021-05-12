using RoboticsManagement.Models;
using System.Collections.Generic;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface ITaskForEmployeeRepository
    {
        public List<TaskForEmployee> GetTaskIdByEmployeeId(string employeeId);
        public List<EmployeeTask> GetTasksForEmployee(string employeeId);
        public void AddNewTaskForEmployee(TaskForEmployee taskForEmployee);
    }
}
