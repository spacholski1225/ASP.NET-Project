using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
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

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();

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
                
                return View(model); //someday there will be logs
            }

            return View(model);
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
                    return RedirectToAction("Error", "Error");
                }
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }
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
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
