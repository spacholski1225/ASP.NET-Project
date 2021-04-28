using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Repositories
{
    public abstract class TaskForEmployeeRepository : ITaskForEmployeeRepository
    {
        public List<int> GetTaskIdByEmployeeId(string employeeId)
        {
            throw new NotImplementedException();
        }

        public EmployeeTask GetTasksForEmployee(string employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
