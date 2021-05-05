using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        [HttpGet]
        public async Task<IActionResult> FinishedTasks()
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
                    CreatedDate = complaintForm.CreatedDate.ToString(),
                    Adress = user.Adress,
                    City = user.City,
                    Company = user.CompanyName,
                    Country = user.Country,
                    ERobotsCategory = complaintForm.ERobotsCategory,
                    NIP = user.NIP,
                    Regon = user.Regon,
                    ZipCode = user.ZipCode,
                    TaskId = taskToGetUser.Id,
                    AppUserId = user.Id
                });
            }
            return View(listToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> FinishedTasks(string userId)
        {
            var complaintForm = _context.complaintFormModels.FirstOrDefault(x => x.ApplicationUser.Id == userId);// it can be deleted if in view
                                                                                                                 // will be properties with hidden attribute
            var user = await _userManager.FindByIdAsync(userId);
            var taskViewModel = new TaskViewModel
            {
                Description = complaintForm.Description,
                CreatedDate = complaintForm.CreatedDate.ToString(),
                Adress = user.Adress,
                City = user.City,
                Company = user.CompanyName,
                Country = user.Country,
                ERobotsCategory = complaintForm.ERobotsCategory,
                NIP = user.NIP,
                Regon = user.Regon,
                ZipCode = user.ZipCode,
                AppUserId = user.Id
            };
            return RedirectToAction("ConcreteTask", "Task", taskViewModel);
        }
        [HttpGet]
        public IActionResult ConcreteTask(TaskViewModel taskViewModel)
        {
            return View(taskViewModel);
        }
        [HttpPost]
        public ContentResult ConcreteTask(TaskViewModel taskViewModel, bool isSure)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TaskViewModel));
            var writer = new StringWriter();
            serializer.Serialize(writer, taskViewModel);
            var xmlString = writer.ToString();
            try
            {
                _context.InvoiceData.Add(new InvoiceData { InvoiceXML = xmlString });
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError("Cannot add invoice date to database!", ex);
            }
            return new ContentResult
            {
                Content = xmlString
            };
        }

    }
}

