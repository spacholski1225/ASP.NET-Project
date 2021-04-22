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
                    return RedirectToAction("Success", "Success"); // TODO change this line into index or sth else after login
                }
                return View(model); //someday there will be logs
            }

            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> CompanyInformation(string name) 
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return RedirectToAction("Error", "Error");
            }
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
    }
}
