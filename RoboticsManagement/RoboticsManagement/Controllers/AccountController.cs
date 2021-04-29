using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogInformation("Unauthenticated user");
                    return View(model);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CompanyRegistration() => View();

        [Authorize(Roles = "Admin")]
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
                    PhoneNumber = model.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.Roles.Any(x => x.Name == "Client"))
                    {
                        var role = new IdentityRole
                        {
                            Name = ERole.Client.ToString()
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, ERole.Client.ToString());
                    return RedirectToAction("Success", "Success");
                }
                else
                {
                    _logger.LogWarning("Can't create Company user, something wrong with User Identity");
                    return RedirectToAction("Error", "Error");
                }
            }
            else
            {
                _logger.LogWarning("While creating user occur invalid model");
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddEmployee() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Adress = model.Adress,
                    Email = model.Email,
                    Country = model.Country,
                    City = model.City,
                    ZipCode = model.ZipCode,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.Roles.Any(x => x.Name == "Employee"))
                    {
                        var role = new IdentityRole
                        {
                            Name = ERole.Employee.ToString()
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(user, ERole.Employee.ToString());
                    return RedirectToAction("Success", "Success");
                }
                else
                {
                    _logger.LogWarning("Can't create Employee user, something wrong with User Identity");
                    return View(model);
                }
            }
            else
            {
                _logger.LogWarning("While creating user occur invalid model");
                return View(model);
            }
        }
    }
}
