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
        private readonly Mock<INotificationRepository> mockNotificationRepository;
        public EmployeeControllerTest()
        {
            mockLogger = new Mock<ILogger<EmployeeController>>();
            mockTaskForEmployeeRepository = new Mock<ITaskForEmployeeRepository>();
            mockEmployeeTaskRepository = new Mock<IEmployeeTaskRepository>();
            mockNotificationRepository = new Mock<INotificationRepository>();

            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null, null, null);
            _controller = new EmployeeController(null, mockUserManager.Object,
                mockTaskForEmployeeRepository.Object, mockEmployeeTaskRepository.Object,
                mockLogger.Object, mockNotificationRepository.Object);
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
        [Fact]
        public async Task TasksForEmployee_ReturnViewResult_ForNullEmployee()
        {
            //Arrange
            string name = "testName";
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            var result = await _controller.TasksForEmployee(name);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(name, viewResult.ViewName);
            mockLogger.Verify(
               x => x.Log(
                   It.Is<LogLevel>(l => l == LogLevel.Warning),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) => true),
                   It.IsAny<Exception>(),
                   It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task TasksForEmployee_ReturnViewResult_ForNotNullEmployeeAndEmptyTasksList()
        {
            //Arrange
            string name = "testName";
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mockTaskForEmployeeRepository.Setup(s => s.GetTasksForEmployee(It.IsAny<string>()))
                .Returns(() => null);
            //Act
            var result = await _controller.TasksForEmployee(name);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }
        [Fact]
        public async Task TasksForEmployee_ReturnViewResult_ForNotNullEmployeeAndTasksList()
        {
            //Arrange
            string name = "testName";
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mockTaskForEmployeeRepository.Setup(s => s.GetTasksForEmployee(It.IsAny<string>()))
                .Returns(new List<EmployeeTask>());
            //Act
            var result = await _controller.TasksForEmployee(name);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<EmployeeTask>>(viewResult.ViewData.Model);
        }
        [Fact]
        public void DoneTask_ReturnRedirectToActionResult_ForTaskEqualNull()
        {
            //Arrange
            int id = 1;
            mockEmployeeTaskRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(() => null);
            //Act
            var result = _controller.DoneTask(id);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToAction.ActionName);
            Assert.Equal("Success", redirectToAction.ControllerName);
            mockLogger.Verify(
               x => x.Log(
                   It.Is<LogLevel>(l => l == LogLevel.Warning),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) => true),
                   It.IsAny<Exception>(),
                   It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public void DoneTask_ReturnRedirectToActionResult_WhentaskIsNotNull()
        {
            //Arrange
            int id = 1;
            mockEmployeeTaskRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask
            { isDone = false });
            mockTaskForEmployeeRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new TaskForEmployee
            {
                EmployeeId = "testId"
            });
            //Act
            var result = _controller.DoneTask(id);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToAction.ActionName);
            Assert.Equal("Success", redirectToAction.ControllerName);
        }
    }
}
