using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }
        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> LoginPage(LoginViewModel model)
        {
           
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = _signInManager.SignInAsync(user, isPersistent: false);

            
            return RedirectToAction("Success", "Success");

        }
        [HttpGet]
        public IActionResult CompanyRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CompanyRegistration(CompanyRegistartionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Country = model.Country,
                    CompanyName = model.CompanyName,
                    City = model.City,
                    Adress = model.Adress,
                    NIP = int.Parse(model.NIP),
                    Regon = int.Parse(model.Regon),
                    ZipCode = model.ZipCode,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Success", "Success");
                }
                else
                {
                    return RedirectToAction("Error", "Error");
                }
            }
            return View(model);

        }
    }
}
