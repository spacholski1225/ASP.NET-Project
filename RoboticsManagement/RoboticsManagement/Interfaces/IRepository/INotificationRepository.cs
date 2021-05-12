using RoboticsManagement.Models;
using RoboticsManagement.Models.Notifications;
using System.Collections.Generic;

namespace RoboticsManagement.Interfaces.IRepository
{
    public interface INotificationRepository
    {
        void AddNotificationsForEmployee(EmployeeNotifications notifi);
        void AddNotificationsForAdmin(AdminNotifications notifi);
        void AddNotificationsForClient(ClientNotifications notifi);
        List<EmployeeNotifications> GetNotificationsForEmployee(string toEmployeeId);
        List<AdminNotifications> GetNotificationsForAdmin(ERole role);
        List<ClientNotifications> GetNotificationsForClient(string toClientId);
        void ReadAllEmployeeNotifications(string toEmployeeId);
        void ReadAllAdminNotifications(ERole role);
        void ReadAllClientNotifications(string toClientId);
        void SetNotificationForEmployeeAsRead(int NotiId);
        void SetNotificationForAdminAsRead(int NotiId);
        void SetNotificationForClientAsRead(int NotiId);
    }
}
