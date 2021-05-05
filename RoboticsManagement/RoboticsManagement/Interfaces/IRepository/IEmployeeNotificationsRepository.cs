using RoboticsManagement.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface IEmployeeNotificationsRepository
    {
        List<EmployeeNotifications> GetNotifications(string toEmployeeId);
        void ReadAllNotifications(string toEmployeeId);
        void SetNotificationAsRead(int NotiId);
    }
}
