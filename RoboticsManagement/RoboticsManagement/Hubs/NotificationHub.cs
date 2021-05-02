using Microsoft.AspNetCore.SignalR;
using RoboticsManagement.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task AddNewNotificationForEmployee(string employeeId, EmployeeNotifications notification)
        {
            // await Clients.User(employeeId).SendAsync("ReceiveTask", notification);
            await Clients.All.SendAsync("ReceiveTask", employeeId,  notification);
        }
    }
}
