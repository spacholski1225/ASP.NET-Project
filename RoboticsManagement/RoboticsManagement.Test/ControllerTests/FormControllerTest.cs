using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Data;
using RoboticsManagement.Interfaces.IRepository;
using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
