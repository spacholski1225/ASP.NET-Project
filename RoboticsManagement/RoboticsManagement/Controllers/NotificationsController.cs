using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeNotificationsRepository _employeeNotificationsRepository;

        public NotificationsController(UserManager<ApplicationUser> userManager,  IEmployeeNotificationsRepository employeeNotificationsRepository)
        {
            _userManager = userManager;
            _employeeNotificationsRepository = employeeNotificationsRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetNotifications(string employeeId = "0506a940-07ea-4162-8e23-235f3f215225", bool isRead = false)
        {
            var notifications = new List<EmployeeNotifications>();
            notifications =_employeeNotificationsRepository.GetNotifications(employeeId, isRead);
            return Json(notifications);
        }
    }
}
