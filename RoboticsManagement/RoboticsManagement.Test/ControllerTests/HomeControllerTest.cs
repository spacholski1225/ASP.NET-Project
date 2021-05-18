using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.Home;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<ILogger<HomeController>> mockLogger;
        private readonly Mock<AutoMapperConfig> mockMapper;
        private readonly Mock<INotificationRepository> _notificationRepository;
        private readonly Mock<IEmployeeTaskRepository> _employeeTaskRepository;
        private readonly Mock<IFormRepository> _formRepository;
        private readonly Mock<IConctactRepository> _contactRepository;

        public HomeControllerTest()
        {
            mockLogger = new Mock<ILogger<HomeController>>();
            mockMapper = new Mock<AutoMapperConfig>();
            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null, null, null);
            _notificationRepository = new Mock<INotificationRepository>();
            _employeeTaskRepository = new Mock<IEmployeeTaskRepository>();
            _formRepository = new Mock<IFormRepository>();
            _contactRepository = new Mock<IConctactRepository>();
            _controller = new HomeController(mockLogger.Object, _notificationRepository.Object,
                mockMapper.Object, mockUserManager.Object, _contactRepository.Object);
        }
        [Fact]
        public async Task Contact_ReturnViewResultWithModel_ForInvalidModel()
        {
            //Arrange
            var model = new ContactViewModel
            {
                Message = "",
                Topic = ""
            };
            _controller.ModelState.AddModelError("error", "some error");
            //Act
            var result = await _controller.Contact(model, "test");
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ContactViewModel>(viewResult.ViewData.Model);
        }
        [Fact]
        public async Task Contact_ReturnRedirectToActionResult_ForValidModel()
        {
            //Arrange
            var model = new ContactViewModel
            {
                Message = "test",
                Topic = "test",
                EContact = Models.Enums.EContact.NotSelected
            };
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser { Id = "test" });
            _contactRepository.Setup(s => s.SaveToDatabase(It.IsAny<Contact>())).Verifiable();
            //Act
            var result = await _controller.Contact(model, "test");
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToActionResult.ActionName);
            Assert.Equal("Success", redirectToActionResult.ControllerName);
        }

    }
}
