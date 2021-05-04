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
        public async Task<IActionResult> FinishedTasksAsync()
        {
            //todo add error handler
            var listToReturn = new List<TaskViewModel>();
            var tasks = new List<TaskForEmployee>(); 

            var doneTasks = _context.EmployeeTasks.Where(x => x.isDone == true).ToList();
            doneTasks.ForEach(t =>
            {
                tasks.Add(_context.TaskForEmployee.FirstOrDefault(x => x.TaskId == t.Id));
            });

            foreach (var task in tasks)
            {
                var taskToGetUser = _context.EmployeeTasks.FirstOrDefault(x => x.Id == task.TaskId);
                var complaintForm = _context.complaintFormModels.FirstOrDefault(x => x.ApplicationUser.Id == taskToGetUser.AppUserId);
                var user = await _userManager.FindByIdAsync(taskToGetUser.AppUserId);
                listToReturn.Add(new TaskViewModel
                {
                    Description = complaintForm.Description,
                    CreatedDate = complaintForm.CreatedDate,
                    Adress = user.Adress,
                    City = user.City,
                    Company = user.CompanyName,
                    Country = user.Country,
                    ERobotsCategory = complaintForm.ERobotsCategory,
                    NIP = user.NIP,
                    Regon = user.Regon,
                    ZipCode = user.ZipCode

                });
            }
            return View(listToReturn);
        }

    }
}

