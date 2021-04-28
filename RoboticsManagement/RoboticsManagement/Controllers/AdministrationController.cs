using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
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
        private readonly IFormRepository _formRepository;

        public AdministrationController(UserManager<ApplicationUser> userManager,
            MgmtDbContext context,
            RoleManager<IdentityRole> roleManager,
            IFormRepository formRepository)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _formRepository = formRepository;
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
        public IActionResult DisplayForm() => View(_formRepository.SortAscById());

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
            if (emp != null)
            {
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
            else
            {
                //todo save into logs
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PickEmployee(string employeeId, int taskId)
        {
            if(employeeId != null && !taskId.Equals(null))
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
            else
            {
                //todo save into logs
            }
            return View();

        }
        [HttpGet]
        public IActionResult NewTask(EmployeeTaskViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public IActionResult NewTask(string employeeId, int taskId) //add something like that user can't get into there without move across all controllers
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
                Console.WriteLine("is already in db" + e.Message); //save into logs or add alert 
            }
            return RedirectToAction("Success", "Success");
        }
    }
}
