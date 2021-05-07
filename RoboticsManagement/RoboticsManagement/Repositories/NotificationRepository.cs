using Microsoft.Extensions.Logging;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models.Notifications;
using System.Collections.Generic;
using System.Linq;
using RoboticsManagement.Models;

namespace RoboticsManagement.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MgmtDbContext _context;
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(MgmtDbContext context, ILogger<NotificationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<AdminNotifications> GetNotificationsForAdmin(ERole role)
        {
            return _context.AdminNotifications.Where(x => x.ToRole.Equals(role) && x.IsRead == false).ToList();
        }
        public void ReadAllAdminNotifications(ERole role)
        {
            var notis = _context.AdminNotifications.Where(x => x.ToRole.Equals(role) && x.IsRead == false).ToList();
            notis.ForEach(n =>
            {
                n.IsRead = true;
            });
            _context.SaveChanges();
        }
        public void SetNotificationForAdminAsRead(int NotiId)
        {
            var noti = _context.AdminNotifications.FirstOrDefault(x => x.NotiId == NotiId);
            if (noti.IsRead == false)
            {
                noti.IsRead = true;
                _context.SaveChanges();
            }
            _logger.LogInformation("Notification with id " + NotiId + "is already read");
        }
        public List<ClientNotifications> GetNotificationsForClient(string toClientId)
        {
            return _context.ClientNotifications.Where(x => x.ToClientId == toClientId && x.IsRead == false).ToList();
        }
        public void ReadAllClientNotifications(string toClientId)
        {
            var notis = _context.ClientNotifications.Where(x => x.ToClientId == toClientId && x.IsRead == false).ToList();
            notis.ForEach(n =>
            {
                n.IsRead = true;
            });
            _context.SaveChanges();
        }
        public void SetNotificationForClientAsRead(int NotiId)
        {
            var noti = _context.ClientNotifications.FirstOrDefault(x => x.NotiId == NotiId);
            if (noti.IsRead == false)
            {
                noti.IsRead = true;
                _context.SaveChanges();
            }
            _logger.LogInformation("Notification with id " + NotiId + "is already read");
        }
        public List<EmployeeNotifications> GetNotificationsForEmployee(string toEmployeeId)
        {
            return _context.EmployeeNotifications.Where(x => x.ToEmployeeId == toEmployeeId && x.IsRead == false).ToList();
        }
        public void ReadAllEmployeeNotifications(string toEmployeeId)
        {
            var notis = _context.EmployeeNotifications.Where(x => x.ToEmployeeId == toEmployeeId && x.IsRead == false).ToList();
            notis.ForEach(n =>
           {
               n.IsRead = true;
           });
            _context.SaveChanges();

        }
        public void SetNotificationForEmployeeAsRead(int NotiId)
        {
            var noti = _context.EmployeeNotifications.FirstOrDefault(x => x.NotiId == NotiId);
            if (noti.IsRead == false)
            {
                noti.IsRead = true;
                _context.SaveChanges();
            }
            _logger.LogInformation("Notification with id " + NotiId + "is already read");

        }
    }
}
