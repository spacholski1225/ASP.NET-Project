using RoboticsManagement.Models.Notifications;
using System.Collections.Generic;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface INotificationRepository
    {
        List<EmployeeNotifications> GetNotificationsForEmployee(string toEmployeeId);
        void ReadAllEmployeeNotifications(string toEmployeeId);
        void SetNotificationForEmployeeAsRead(int NotiId);
    }
}
