using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class TaskControllerTest
    {
        private readonly TaskController _controller;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<ILogger<TaskController>> mockLogger;
        private readonly Mock<AutoMapperConfig> mockMapper;
        private readonly Mock<INotificationRepository> _notificationRepository;
        private readonly Mock<IEmployeeTaskRepository> _employeeTaskRepository;
        private readonly Mock<IFormRepository> _formRepository;
        private readonly Mock<ITaskForEmployeeRepository> _taskForEmployeeRepository;

        public TaskControllerTest()
        {
            mockLogger = new Mock<ILogger<TaskController>>();
            mockMapper = new Mock<AutoMapperConfig>();
            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null, null, null);
            _notificationRepository = new Mock<INotificationRepository>();
            _employeeTaskRepository = new Mock<IEmployeeTaskRepository>();
            _formRepository = new Mock<IFormRepository>();
            _taskForEmployeeRepository = new Mock<ITaskForEmployeeRepository>();
            _controller = new TaskController(mockLogger.Object, null, mockUserManager.Object,
                mockMapper.Object, _employeeTaskRepository.Object, _formRepository.Object,
                _taskForEmployeeRepository.Object);
        }
        [Fact]
        public async Task FinishedTask_ReturnBadRequest_IfListOfTaskIsEmpty()
        {
            //Arrange
            _employeeTaskRepository.Setup(s => s.GetDoneTasks()).Returns(() => null);
            //Act
            var result = await _controller.FinishedTasks();
            //Assert
            var request = Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public async Task FinishedTask_ReturnViewResult_WhenEverythingIsOkay()
        {
            //Arrange
            _employeeTaskRepository.Setup(s => s.GetDoneTasks()).Returns(new List<EmployeeTask>
            {
                new EmployeeTask{Id = 1}
            });
            _employeeTaskRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new EmployeeTask
            {
                Id = 1
            });
            _taskForEmployeeRepository.Setup(s => s.GetTaskById(It.IsAny<int>())).Returns(new TaskForEmployee
            {
                TaskId = 1
            });
            _formRepository.Setup(s => s.GetFormByUserId(It.IsAny<string>())).Returns(new FormModel
            {
                ERobotsCategory = ERobotsCategory.Robot1,
                Description = "test",
                CreatedDate = DateTime.Now
            });
            mockUserManager.Setup(s => s.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            //Act
            var result = await _controller.FinishedTasks();
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData.Model);
            Assert.IsType<List<TaskViewModel>>(viewResult.ViewData.Model);
        }
        [Fact]
        public async Task FinishedTasksPOST_ReturnedBadRequest_ForUserIdEqualNull()
        {
            //Arrange
            string userId = null;
            //Act
            var result = await _controller.FinishedTasks(userId);
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
