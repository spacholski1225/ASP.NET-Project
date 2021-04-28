using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Repositories
{
    public abstract class EmployeeTaskRepository : IEmployeeTaskRepository
    {
        public EmployeeTask GetByTaskId(int taskId)
        {
            throw new NotImplementedException();
        }

        public EmployeeTask GetDoneTasks()
        {
            throw new NotImplementedException();
        }

        public EmployeeTask GetUnDoneTasks()
        {
            throw new NotImplementedException();
        }
    }
}
