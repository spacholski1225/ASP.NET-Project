using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
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
    public class AdministrationControllerTest
    {
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly AdministrationController _controller;
        private readonly Mock<IEmployeeTaskRepository> mockRepository;
        private readonly Mock<ILogger<AdministrationController>> mockLogger;
        private readonly Mock<AutoMapperConfig> mockMapper;
        private readonly Mock<INotificationRepository> mockNotificationRepository;
        private readonly Mock<ITaskForEmployeeRepository> mockTaskForEmployeeRepository;

        public AdministrationControllerTest()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            mockRepository = new Mock<IEmployeeTaskRepository>();
            mockLogger = new Mock<ILogger<AdministrationController>>();
            mockMapper = new Mock<AutoMapperConfig>();
            mockNotificationRepository = new Mock<INotificationRepository>();
            mockTaskForEmployeeRepository = new Mock<ITaskForEmployeeRepository>();
            _controller = new AdministrationController(mockUserManager.Object, null, null,
                mockRepository.Object, mockLogger.Object, mockMapper.Object, mockNotificationRepository.Object,
                mockTaskForEmployeeRepository.Object);
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
        [Fact]
        public async Task PickEmployee_ReturnViewResult_WhenEmployeeIdOrTaskIdEqualNull()
        {
            //Arrange
            string employeeId = null;
            int taskId = 0;
            //Act
            var result = await _controller.PickEmployee(employeeId, taskId);
            //Assert
            Assert.IsType<ViewResult>(result);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "While sending task to employee occurs error where employeeId or taskId null"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task PickEmployee_ReturnViewResult_WhenEmployeeIdAndTaskIdIsNotNullAndEmployeeAndTaskExist()
        {
            //Arrange
            string employeeId = "test";
            int taskId = 1;

            mockUserManager.Setup(s => s.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask { Id = taskId });
            //Act
            var result = await _controller.PickEmployee(employeeId, taskId);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("NewTask", redirectToAction.ActionName);
            Assert.NotNull(redirectToAction.RouteValues);

        }
        [Fact]
        public async Task PickEmployee_ReturnViewResult_WhenEmployeeIdAndTaskIdIsNotNullAndEmployeeOrTaskDoesNotExist()
        {
            //Arrange
            string employeeId = "test";
            int taskId = 1;

            mockUserManager.Setup(s => s.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask { Id = taskId });

            //Act
            var result = await _controller.PickEmployee(employeeId, taskId);
            //Assert
            Assert.IsType<ViewResult>(result);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Can not find task with id " + taskId + " or employee with id " + employeeId),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

        }
        [Fact]
        public void NewTask_ReturnRedirectToActionResult_ForEmployeeIdEqualNull()
        {
            //Arrange
            string employeeId = null;
            int taskId = 0;
            //Act
            var result = _controller.NewTask(employeeId, taskId);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Error", redirectToAction.ActionName);
            Assert.Equal("Error", redirectToAction.ControllerName);
        }
        [Fact]
        public void NewTask_ReturnRedirectToActionResult_WhenOccursError()
        {
            //Arrange
            string employeeId = "test";
            int taskId = 1;
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask
            {
                Id = taskId,
                Description = "test",
                Company = "test"
            });
            mockNotificationRepository.Setup(s => s.AddNotificationsForEmployee(It.IsAny<EmployeeNotifications>()))
                .Throws(new Exception());
            //Act
            var result = _controller.NewTask(employeeId, taskId);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Error", redirectToAction.ActionName);
            Assert.Equal("Error", redirectToAction.ControllerName);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public void NewTask_ReturnRedirectToActionResult_WhenIsCorrectSave()
        {
            //Arrange
            string employeeId = "test";
            int taskId = 1;
            mockRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask
            {
                Id = taskId,
                Description = "test",
                Company = "test"
            });
            mockNotificationRepository.Setup(s => s.AddNotificationsForEmployee(It.IsAny<EmployeeNotifications>()))
                .Verifiable();
            mockTaskForEmployeeRepository.Setup(s => s.AddNewTaskForEmployee(It.IsAny<TaskForEmployee>()))
                .Verifiable();
            //Act
            var result = _controller.NewTask(employeeId, taskId);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToAction.ActionName);
            Assert.Equal("Success", redirectToAction.ControllerName);
        }
    }
}
