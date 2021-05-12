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
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class CompanyControllerTest
    {
        private readonly CompanyController _controller;
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly Mock<ILogger<CompanyController>> mockLogger;
        private readonly Mock<IFormRepository> mockFormRepository;
        private readonly Mock<AutoMapperConfig> mockMapper;
        public CompanyControllerTest()
        {
            mockLogger = new Mock<ILogger<CompanyController>>();
            mockFormRepository = new Mock<IFormRepository>();
            mockMapper = new Mock<AutoMapperConfig>();

            var mockIUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockIUserStore.Object,
                null, null, null, null, null, null,null,null);
            _controller = new CompanyController(mockUserManager.Object, mockLogger.Object,
                mockFormRepository.Object, mockMapper.Object);
        }

    }
}
