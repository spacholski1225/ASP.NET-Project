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
    }
}
