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
            var listToReturn = new List<TaskViewModel>(); // lista z danymi Company
            var tasks = new List<FormModel>(); //do zmiany <- tabela TaskForEmployee zwierajaca Id skonczonego zadania wraz z osoba ktora je zrobila
            var doneTasks = _context.EmployeeTasks.Where(x => x.isDone == true).ToList();
            doneTasks.ForEach(t =>
            {
             //dla kazdego taska szukamy EmployeeId w TaskForEmployee a nastepnie d zapisaujemy to  do listy tasks
            });
            tasks.ForEach(t =>
            {
                //tutaj powinna znalezc sie tworzenie listy z danymi Company znaleznione poprzez wykonane zadanie z listy tasks
                //trzeba zmienic strukture tabel tak aby EmployeeTasks zawieralo Id firmy ktora stworzyla to zadanie, albo zmiana jakos w logice bo teraz 
                //wydaje mi sie ze nie da sie dostac do aktualnego Id w ApplicationUserId w tabeli complaintFormModels
            });
            return View();
        }

    }
}

