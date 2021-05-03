using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly MgmtDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ILogger<TaskController> logger, MgmtDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> FinishedTasks()
        {
            var listToReturn = new List<TaskViewModel>();
            var tasks = new List<FormModel>();
            var doneTasks = _context.EmployeeTasks.Where(x => x.isDone == true).ToList();
            doneTasks.ForEach(t =>
            {
                tasks.Add(_context.complaintFormModels.FirstOrDefault(c => (c.Id == t.Id) && (c.ApplicationUser.Id != null)));
            });
            tasks.ForEach(t =>
            {
                var user = _userManager.FindByIdAsync(t.ApplicationUser.Id);
            });
            //do skonczenia po naprawieniu dodawania zadania przez administrationcontroller
            return View();
        }

    }
}

