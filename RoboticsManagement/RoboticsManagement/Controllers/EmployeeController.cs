using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using System;
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
            if(task == null)
            {
                return RedirectToAction("Error", "Error"); //add into logs
            }
            return View(task);
        }
        [HttpGet]
        public async Task<IActionResult> TasksForEmployee(string name)
        {
            var employee = await _userManager.FindByNameAsync(name);
            if(employee != null)
            {

            }
        }
        public IActionResult DoneTask(int id)
        {
            var task = _context.EmployeeTasks.FirstOrDefault(x => x.Id == id);
            if(task != null)
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
