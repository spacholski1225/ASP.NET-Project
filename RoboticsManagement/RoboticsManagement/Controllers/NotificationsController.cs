using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeNotificationsRepository _employeeNotificationsRepository;

        public NotificationsController(UserManager<ApplicationUser> userManager,  IEmployeeNotificationsRepository employeeNotificationsRepository)
        {
            _userManager = userManager;
            _employeeNotificationsRepository = employeeNotificationsRepository;
        }
        public async Task<IActionResult> Index(string empName)
        {
            var user = await _userManager.FindByNameAsync(empName);
            var notis = _employeeNotificationsRepository.GetNotifications(user.Id);
            return View(notis);
        }
        [HttpPost]
        public JsonResult GetNotifications(string employeeId = "0506a940-07ea-4162-8e23-235f3f215225")
        {
            var notifications = new List<EmployeeNotifications>();
            notifications =_employeeNotificationsRepository.GetNotifications(employeeId);
            return Json(notifications);
        }

        
    }
}
