using Microsoft.Extensions.Logging;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace RoboticsManagement.Repositories
{
    public class EmployeeNotificationsRepository : IEmployeeNotificationsRepository
    {
        private readonly MgmtDbContext _context;
        private readonly ILogger<EmployeeNotificationsRepository> _logger;

        public EmployeeNotificationsRepository(MgmtDbContext context, ILogger<EmployeeNotificationsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<EmployeeNotifications> GetNotifications(string toEmployeeId, bool isRead)
        {
            return _context.EmployeeNotifications.Where(x => x.ToEmployeeId == toEmployeeId && x.IsRead == isRead).ToList();
        }

        public void ReadAllNotifications(string toEmployeeId)
        {
            var notis = _context.EmployeeNotifications.Where(x => x.ToEmployeeId == toEmployeeId && x.IsRead == false).ToList();
            notis.ForEach(n =>
           {
               n.IsRead = true;
           });
            _context.SaveChanges();

        }

        public void SetNotificationAsRead(int NotiId)
        {
            var noti = _context.EmployeeNotifications.FirstOrDefault(x => x.NotiId == NotiId);
            if(noti.IsRead == false)
            {
                noti.IsRead = true;
                _context.SaveChanges();
            }
            _logger.LogInformation("Notification with id " + NotiId + "is already read");

        }
    }
}
