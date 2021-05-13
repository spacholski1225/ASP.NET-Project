using Microsoft.AspNetCore.Identity;
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
            _controller = new TaskController(mockLogger.Object, null, mockUserManager.Object,
                mockMapper.Object);
        }
    }
}
