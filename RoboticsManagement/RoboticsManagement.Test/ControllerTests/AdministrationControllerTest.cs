using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Controllers;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
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
        public AdministrationControllerTest()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            mockRepository = new Mock<IEmployeeTaskRepository>();
            mockLogger = new Mock<ILogger<AdministrationController>>();
            _controller = new AdministrationController(mockUserManager.Object, null, null, mockRepository.Object, mockLogger.Object, null);
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

    }
}
