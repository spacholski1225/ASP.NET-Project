using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface ITaskForEmployeeRepository
    {
        public List<int> GetTaskIdByEmployeeId(string employeeId);
        public EmployeeTask GetTasksForEmployee(string employeeId);
    }
}
