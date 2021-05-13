using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.Models.Notifications;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class FormControllerTest
    {
        private readonly FormController _controller;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<ILogger<FormController>> mockLogger;
        private readonly Mock<AutoMapperConfig> mockMapper;
        private readonly Mock<INotificationRepository> _notificationRepository;
        private readonly Mock<IEmployeeTaskRepository> _employeeTaskRepository;
        private readonly Mock<IFormRepository> _formRepository;

        public FormControllerTest()
        {
            mockLogger = new Mock<ILogger<FormController>>();
            mockMapper = new Mock<AutoMapperConfig>();
            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null, null, null);
            _notificationRepository = new Mock<INotificationRepository>();
            _employeeTaskRepository = new Mock<IEmployeeTaskRepository>();
            _formRepository = new Mock<IFormRepository>();
            _controller = new FormController(null, mockUserManager.Object,
                mockLogger.Object, mockMapper.Object, _notificationRepository.Object,
                _employeeTaskRepository.Object, _formRepository.Object);
        }
        [Fact]
        public async Task Form_ReturnsViewResult_ForInvalidModelOrNameEqualNull()
        {
            //Arrange
            var model = new FormViewModel();
            //Act
            var result = await _controller.Form(model, null);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<FormViewModel>(viewResult.ViewData.Model);
        }
        [Fact]
        public async Task Form_ReturnsViewResultAndLogWarning_ForUnKnownUser()
        {
            //Arrange
            var model = new FormViewModel();
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            var result = await _controller.Form(model, "test");
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<FormViewModel>(viewResult.ViewData.Model);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task Form_ReturnsRedirectToAction_ForUser()
        {
            //Arrange
            var model = new FormViewModel
            {
                ERobotsCategory = ERobotsCategory.Robot1,
                Description = "test"
            };
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            //Act
            var result = await _controller.Form(model, "test");
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Summary", redirectToActionResult.ActionName);
            Assert.Equal("Form", redirectToActionResult.ControllerName);

        }
        [Fact]
        public async Task Summary_ReturnViewResult_ForInValidModelOrIsnotOkay()
        {
            //Arrange
            var model = new SummaryViewModel();
            bool isOkay = false;
            //Act
            var result = await _controller.Summary(model, isOkay);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SummaryViewModel>(viewResult.ViewData.Model);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task Summary_RedirectToActionResult_ForValidModelAndIsOkay()
        {
            //Arrange
            var model = new SummaryViewModel
            {
                UserId = "test",
                Company = "test"
            };
            bool isOkay = true;
            mockUserManager.Setup(s => s.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            _notificationRepository.Setup(s => s.AddNotificationsForClient(It.IsAny<ClientNotifications>()))
                .Verifiable();
            _notificationRepository.Setup(s => s.AddNotificationsForAdmin(It.IsAny<AdminNotifications>()))
                .Verifiable();
            _employeeTaskRepository.Setup(s => s.AddEmployeeTask(It.IsAny<EmployeeTask>())).Verifiable();
            _formRepository.Setup(s => s.AddTask(It.IsAny<FormModel>())).Verifiable();
            //Act
            var result = await _controller.Summary(model, isOkay);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToActionResult.ActionName);
            Assert.Equal("Success", redirectToActionResult.ControllerName);

        }

    }
}
