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
    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<ILogger<HomeController>> mockLogger;
        private readonly Mock<AutoMapperConfig> mockMapper;
        private readonly Mock<INotificationRepository> _notificationRepository;
        private readonly Mock<IEmployeeTaskRepository> _employeeTaskRepository;
        private readonly Mock<IFormRepository> _formRepository;

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
            _controller = new HomeController(mockLogger.Object, _notificationRepository.Object,
                mockMapper.Object, mockUserManager.Object);
        }
    }
}
