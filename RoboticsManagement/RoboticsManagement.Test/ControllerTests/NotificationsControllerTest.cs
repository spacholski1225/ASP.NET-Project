using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels.Notifications;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class NotificationsControllerTest
    {
        private readonly NotificationsController _controller;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<INotificationRepository> mockNotiRepository;
        private readonly Mock<AutoMapperConfig> mockMapper;
        public NotificationsControllerTest()
        {
            mockNotiRepository = new Mock<INotificationRepository>();
            mockMapper = new Mock<AutoMapperConfig>();

            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null, null, null);
            _controller = new NotificationsController(mockUserManager.Object, mockNotiRepository.Object,
                mockMapper.Object);
        }

        [Fact]
        public async Task GetNotifications_ReturnWelcomePage_ForNameEqualNull()
        {
            //Arrange
            string name = null;
            //Act
            var result = await _controller.GetNotifications(name);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Welcome", redirectToAction.ActionName);
            Assert.Equal("Success", redirectToAction.ControllerName);
        }
        [Fact]
        public async Task GetNotifications_ReturnRedirectToActin_IfUserIsHaveNotARole()
        {
            //Arrange
            mockUserManager.Setup(s => s.IsInRoleAsync(new ApplicationUser(), It.IsAny<string>())).ReturnsAsync(() => false);

            //Act
            var result = await _controller.GetNotifications("test");
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Error", redirectToActionResult.ActionName);
            Assert.Equal("Error", redirectToActionResult.ControllerName);
        }

    }
}
