using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly MgmtDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(MgmtDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult EmployeeTask(int id)
        {
            var task = _context.EmployeeTasks.FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return RedirectToAction("Error", "Error"); //add into logs
            }
            return View(task);
        }
        [HttpGet]
        public async Task<IActionResult> TasksForEmployee(string name)
        {
            var employee = await _userManager.FindByNameAsync(name);
            if (employee != null)
            {
                var listOfTasks = new List<EmployeeTask>();
                var tasks = _context.TaskForEmployee.Where(x => x.EmployeeId == employee.Id).ToList();
                tasks.ForEach(t =>
                {
                    var task = _context.EmployeeTasks.FirstOrDefault(x => (x.Id == t.TaskId && x.isDone == false));
                    if (task != null)
                        listOfTasks.Add(task);
                });

                return View(listOfTasks);
            }
            return View(name); //add logs
        }
        public IActionResult DoneTask(int id)
        {
            var task = _context.EmployeeTasks.FirstOrDefault(x => x.Id == id);
            if (task != null)
            {
                task.isDone = true;
                _context.SaveChanges();
            }
            else
            {
                //save to logs
            }
            return RedirectToAction("Success", "Success");

        }
    }
}
