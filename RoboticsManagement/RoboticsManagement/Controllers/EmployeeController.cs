using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Data;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MgmtDbContext _context;

        public EmployeeController(MgmtDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult EmployeeTask(int id)
        {
            var task = _context.EmployeeTasks.FirstOrDefault(x => x.Id == id);
            return View(task);
        }
        [HttpGet]
        public IActionResult TasksForEmployee()
        {
            var tasks = _context.EmployeeTasks.OrderBy(x => x.Id).ToList();
            return View(tasks);
        }
        public IActionResult DoneTask(int id)
        {
            var task = _context.EmployeeTasks.FirstOrDefault(x => x.Id == id);
            try
            {
                _context.EmployeeTasks.Remove(task);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("Success", "Success");

        }
    }
}
