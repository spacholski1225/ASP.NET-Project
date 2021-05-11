using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
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
    public class AdministrationControllerTest
    {
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly AdministrationController _controller;
        private readonly Mock<IEmployeeTaskRepository> mockRepository;
        private readonly Mock<ILogger<AdministrationController>> mockLogger;
        private readonly Mock<AutoMapperConfig> mockMapper;
        public AdministrationControllerTest()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            mockRepository = new Mock<IEmployeeTaskRepository>();
            mockLogger = new Mock<ILogger<AdministrationController>>();
            mockMapper = new Mock<AutoMapperConfig>();
            _controller = new AdministrationController(mockUserManager.Object, null, null, mockRepository.Object, mockLogger.Object, mockMapper.Object);
        }
        [Fact]
        public void DisplayForm_ResultRedirectToActionResult_ForExistRecord()
        {
            //Arrange
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask());
            //Act
            var result = _controller.DisplayForm(2);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ConcreteForm", redirectToActionResult.ActionName);
            Assert.Equal("Administration", redirectToActionResult.ControllerName);
        }
        [Fact]
        public void DisplayForm_ResultRedirectToActionResult_ForDoesNotExistRecord()
        {
            //Arrange
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(() => null);
            //Act
            var result = _controller.DisplayForm(2);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("DisplayForm", redirectToActionResult.ActionName);
            Assert.Equal("Administration", redirectToActionResult.ControllerName);
        }
        [Fact]
        public void ConcreteForm_ReturnRedirectToActionResult_ForExistRecord()
        {
            //Arrange
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask { Id = 2 });
            //Act
            var result = _controller.ConcreteForm(2);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("PickEmployee", redirectToActionResult.ActionName);
            Assert.NotNull(redirectToActionResult.RouteValues);
        }
        [Fact]
        public void ConcreteForm_ReturnRedirectToActionResult_ForDoesNotExistRecord()
        {
            //Arrange
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(() => null);
            //Act
            var result = _controller.ConcreteForm(2);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ConcreteForm", redirectToActionResult.ActionName);
            Assert.Equal("Administration", redirectToActionResult.ControllerName);
        }
        [Fact]
        public async Task PickEmployee_ReturnViewResult_WhenUserIsInRole()
        {
            //Arrange
            IList<ApplicationUser> returned = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "testId"}
            };
            mockUserManager.Setup(s => s.GetUsersInRoleAsync(It.IsAny<string>())).ReturnsAsync(returned);
            //Act
            var result = await _controller.PickEmployee(new EmployeeTaskViewModel { TaskId = 1 });
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<EmployeeTaskViewModel>>(viewResult.ViewData.Model);

        }
        [Fact]
        public async Task PickEmployee_ReturnViewResult_WhenUserIsNotInRole()
        {
            //Arrange
            IList<ApplicationUser> returned = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "testId"}
            };
            mockUserManager.Setup(s => s.GetUsersInRoleAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            var result = await _controller.PickEmployee(new EmployeeTaskViewModel { TaskId = 1 });
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Can't find employees in Employee role, AdministrationController PickEmployee method"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
    }
}
