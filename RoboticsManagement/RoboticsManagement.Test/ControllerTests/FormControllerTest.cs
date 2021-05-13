using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
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
        
        public FormControllerTest()
        {
            mockLogger = new Mock<ILogger<FormController>>();
            mockMapper = new Mock<AutoMapperConfig>();
            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null, null, null);
            _controller = new FormController(null, mockUserManager.Object,
                mockLogger.Object, mockMapper.Object);
        }
        [Fact]
        public async Task Form_ReturnsViewResult_ForInvalidModelOrNameEqualNull()
        {
            //Arrange
            var model = new FormViewModel();
            //Act
            var result = await _controller.Form(model,null);
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
    }
}
