using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Configuration;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
using System.Threading.Tasks;

namespace RoboticsManagement.Controllers
{

    public class HomeController : Controller
    {
        private readonly AutoMapperConfig _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, INotificationRepository notificationRepository,
            AutoMapperConfig mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet]
        [Route("~/Shared/Index.cshtml")]
        public IActionResult Index() => RedirectToAction("GetNotifications","Notifications");

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel model, string userName)
        {
            if (ModelState.IsValid)
            {
                var mapModel = _mapper.MapContactViewModelToContact(model);
                if (userName == null)
                {
                    mapModel.UserName = "Undefined User";
                }
                return Json(mapModel);
            }
            return NotFound();
        }
    }
}
