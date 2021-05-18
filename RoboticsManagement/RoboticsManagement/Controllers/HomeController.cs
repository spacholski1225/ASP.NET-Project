using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoboticsManagement.Configuration;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.Notifications;
using RoboticsManagement.ViewModels;
using System;
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
        public IActionResult Index() => RedirectToAction("GetNotifications", "Notifications");

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model, string userName)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                var mapModel = _mapper.MapContactViewModelToContact(model);
                if (userName == null)
                {
                    mapModel.UserName = "Undefined User";
                }
                try
                {
                    user = await _userManager.FindByNameAsync(userName);
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Cannot find user");
                }
                if (mapModel.EContact.ToString().Equals("SendToAdmins"))
                {
                    mapModel.Sender = mapModel.UserName;
                    mapModel.Receiver = ERole.Admin.ToString();

                    _notificationRepository.AddNotificationsForAdmin(new AdminNotifications
                    {
                        CreatedDate = DateTime.Now,
                        FromUserId = user.Id,
                        IsRead = false,
                        NotiBody = mapModel.Message,
                        NotiHeader = "New message from " + mapModel.UserName,
                        ToRole = ERole.Admin
                    });
                    //stworzyc contactrepository
                    //z zapisem message do bazy danych
                    return RedirectToAction("Success", "Success");
                    //dodac podsumowanie jaka wiadomosc zostala wyslana i jesli uzytkownik ma konto
                    //to niech dostanie powiadomienie z podsumowaniem wiadomosci

                    //dodac if Send To Concrete Employee przekierowanie na AllEmployees.cshtml
                    //tak samo z Client
                }
            }
            return RedirectToAction("Error", "Error");
        }
    }
}