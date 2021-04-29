﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize(Roles = "Client")]
    public class CompanyController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CompanyController> _logger;
        private readonly IFormRepository _formRepository;

        public CompanyController(UserManager<ApplicationUser> userManager,
            ILogger<CompanyController> logger,
            IFormRepository formRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _formRepository = formRepository;
        }
        [HttpGet]
        public async Task<IActionResult> CompanyInformation(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                var model = new CompanyInfoViewModel
                {
                    Email = user.Email,
                    Country = user.Country,
                    CompanyName = user.CompanyName,
                    City = user.City,
                    Adress = user.Adress,
                    NIP = user.NIP.ToString(),
                    Regon = user.Regon.ToString(),
                    ZipCode = user.ZipCode,
                    PhoneNumber = user.PhoneNumber
                };
                return View(model);
            }
            else
            {
                _logger.LogWarning("Usermanager can't find a name " + name); 
                return RedirectToAction("Error", "Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> SentForms(string name)
        {

            var forms = await _formRepository.GetAllFormsByUser(name);
            var sentForms = new List<SentFormsViewModel>();

            foreach (var form in forms)
            {
                sentForms.Add(new SentFormsViewModel
                {
                    Description = form.Description,
                    CreatedDate = form.CreatedDate,
                    Robot = form.ERobotsCategory.ToString()
                });
            }
            

           return View(sentForms);
        }

    }
}
