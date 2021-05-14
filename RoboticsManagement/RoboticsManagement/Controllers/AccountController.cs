using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Configuration;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using RoboticsManagement.Models.Notifications;
using RoboticsManagement.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly AutoMapperConfig _mapper;
        private readonly MgmtDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger,
            AutoMapperConfig mapper,
            MgmtDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("GetNotifications", "Notifications");
            }
            return View();
        }

        [AllowAnonymous]
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

        [HttpGet]
        public IActionResult CompanyRegistration() => View();

        [HttpPost]
        public async Task<IActionResult> CompanyRegistration(CompanyRegistartionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.MapCompanyRegistartionViewModelToApplicationUser(model);
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

                    var noti = new ClientNotifications
                    {
                        CreatedDate = DateTime.Now,
                        ToClientId = user.Id,
                        FromRole = ERole.Admin,
                        IsRead = false,
                        NotiBody = "Hello " + user.CompanyName + " in our system!",
                        NotiHeader = "Welcome ;)"
                    };
                    try
                    {
                        _context.ClientNotifications.Add(noti);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error within saveing notification after registration", ex);
                    }
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

        [HttpGet]
        public IActionResult AddEmployee() => View();

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.MapEmployeeRegistrationViewModelToApplicationUser(model);
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
