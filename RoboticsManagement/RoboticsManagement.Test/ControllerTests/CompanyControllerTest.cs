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
        [Fact]
        public async Task CompanyInformation_ReturnsRedirectToActionResult_ForCompanyEqualNull()
        {
            //Arrange
            string name = "testName";
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            var result = await _controller.CompanyInformation(name);
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
        public async Task CompanyInformation_ReturnsViewResult_ForCompanyEqualNotNull()
        {
            //Arrange
            string name = "testName";
            mockUserManager.Setup(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            //Act
            var result = await _controller.CompanyInformation(name);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<CompanyInfoViewModel>(viewResult.ViewData.Model);
        }
        [Fact]
        public async Task SentForms_ReturnsRedirectToAction_ForFormsEqualNull()
        {
            //Arrange
            string name = "testName";
            mockFormRepository.Setup(s => s.GetAllFormsByUser(It.IsAny<string>())).ReturnsAsync(() => null);
            //Act
            var result = await _controller.SentForms(name);
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
        public async Task SentForms_ReturnsViewResult_ForFormsNotNull()
        {
            //Arrange
            string name = "testName";
            mockFormRepository.Setup(s => s.GetAllFormsByUser(It.IsAny<string>()))
                .ReturnsAsync(new List<FormModel>());
            //Act
            var result = await _controller.SentForms(name);
            //Assert
            var resultView= Assert.IsType<ViewResult>(result);
            Assert.IsType<List<SentFormViewModel>>(resultView.ViewData.Model);
        }
    }
}
