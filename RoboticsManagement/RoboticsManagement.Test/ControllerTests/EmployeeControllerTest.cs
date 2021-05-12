using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
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
    public class EmployeeControllerTest
    {
        private readonly EmployeeController _controller;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<ILogger<EmployeeController>> mockLogger;
        private readonly Mock<IEmployeeTaskRepository> mockEmployeeTaskRepository;
        private readonly Mock<ITaskForEmployeeRepository> mockTaskForEmployeeRepository;
        public EmployeeControllerTest()
        {
            mockLogger = new Mock<ILogger<EmployeeController>>();
            mockTaskForEmployeeRepository = new Mock<ITaskForEmployeeRepository>();
            mockEmployeeTaskRepository = new Mock<IEmployeeTaskRepository>();

            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null, null, null);
            _controller = new EmployeeController(null, mockUserManager.Object,
                mockTaskForEmployeeRepository.Object, mockEmployeeTaskRepository.Object,
                mockLogger.Object);
        }
        [Fact]
        public void EmployeeTask_ReturnsRedirectToAction_ForTaskEqualNull()
        {
            //Arrange
            int id = 1;
            mockEmployeeTaskRepository.Setup(s => s.GetTaskById(It.IsAny<int>()))
                .Returns(() => null);
            //Act
            var result = _controller.EmployeeTask(id);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Error", redirectToAction.ActionName);
            Assert.Equal("Error", redirectToAction.ControllerName);
            mockLogger.Verify(
               x => x.Log(
                   It.Is<LogLevel>(l => l == LogLevel.Warning),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) => true),
                   It.IsAny<Exception>(),
                   It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public void EmployeeTask_ReturnsViewResult_ForTaskNotNull()
        {
            //Arrange
            int id = 1;
            mockEmployeeTaskRepository.Setup(s => s.GetTaskById(It.IsAny<int>()))
                .Returns(new EmployeeTask());
            //Act
            var result = _controller.EmployeeTask(id);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<EmployeeTask>(viewResult.ViewData.Model);
        }
    }
}
