using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MgmtDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController(UserManager<ApplicationUser> userManager, MgmtDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> AddAdmin()
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin"

                };
                var result = await _userManager.CreateAsync(user, "zaq1@WSX");
                if (result.Succeeded)
                {
                    if (!_roleManager.Roles.Any(x => x.Name == "Admin"))
                    {
                        var role = new IdentityRole
                        {
                            Name = ERole.Admin.ToString()
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, ERole.Admin.ToString());
                    return RedirectToAction("Success", "Success");
                }
            }
            return Ok();

        }//to delete or modify

        [HttpGet]
        public IActionResult DisplayForm()
        {
            var forms = _context.complaintFormModels.OrderBy(x => x.Id).ToList();

            return View(forms);
        }

        [HttpPost]
        public IActionResult DisplayForm(int id)
        {
            var result = _context.complaintFormModels.FirstOrDefault(x => x.Id == id);
            return RedirectToAction("ConcreteForm", "Administration", result);
        }

        [HttpGet]
        public IActionResult ConcreteForm(FormModel result)
        {
            return View(result);
        }
        [HttpPost]
        public IActionResult ConcreteForm(int id)
        {
            var task = _context.complaintFormModels.FirstOrDefault(x => x.Id == id);
            var newTask = new EmployeeTaskViewModel
            {
                TaskId = task.Id,
            };
            return RedirectToAction("PickEmployee", newTask);
        }
        [HttpGet]
        public async Task<IActionResult> PickEmployee(EmployeeTaskViewModel newTask)
        {
            var emp = await _userManager.GetUsersInRoleAsync("Employee");
            var employees = new List<EmployeeTaskViewModel>();
            foreach (var employee in emp)
            {
                employees.Add(new EmployeeTaskViewModel
                {
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    TaskId = newTask.TaskId
                });
            }
            return View(employees);
        }
        [HttpPost]
        public async Task<IActionResult> PickEmployee(string employeeId, int taskId)//todo add handler exceptions
        {
            var employee = await _userManager.FindByIdAsync(employeeId);
            var task = _context.complaintFormModels.FirstOrDefault(x => x.Id == taskId);


            var model = new EmployeeTaskViewModel
            {
                Description = task.Description,
                Adress = task.Adress,
                City = task.City,
                Company = task.Company,
                Country = task.Country,
                EmployeeId = employeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                TaskId = taskId,
                ZipCode = task.ZipCode

            };
            return RedirectToAction("NewTask", model);
        }
        [HttpGet]
        public IActionResult NewTask(EmployeeTaskViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public IActionResult NewTask(string employeeId, int taskId)
        {
            var entity = new TaskForEmployee
            {
                EmployeeId = employeeId,
                TaskId = taskId
            };
            try
            {
                _context.TaskForEmployee.Add(entity); //error when id is already in database
                _context.SaveChanges();//fix error when id is already in database
            }
            catch (Exception e)
            {
                Console.WriteLine("is already in db" + e.Message); //save in log or add alert 
            }
            return RedirectToAction("Success", "Success");
        }
    }
}
