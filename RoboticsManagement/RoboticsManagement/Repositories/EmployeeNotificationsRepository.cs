using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Repositories
{
    public class EmployeeNotificationsRepository : IEmployeeNotificationsRepository
    {
        public List<EmployeeNotifications> GetNotifications(string toEmployeeId, bool isRead)
        {
            throw new NotImplementedException();
        }
    }
}
