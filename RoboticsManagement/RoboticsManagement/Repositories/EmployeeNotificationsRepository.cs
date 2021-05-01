using RoboticsManagement.Data;
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
        private readonly MgmtDbContext _context;

        public EmployeeNotificationsRepository(MgmtDbContext context)
        {
            _context = context;
        }
        public List<EmployeeNotifications> GetNotifications(string toEmployeeId, bool isRead)
        {
            return _context.EmployeeNotifications.Where(x => x.ToEmployeeId == toEmployeeId && x.IsRead == isRead).ToList();
        }
    }
}
