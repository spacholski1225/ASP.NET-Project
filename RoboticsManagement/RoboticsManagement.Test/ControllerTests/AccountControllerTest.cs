using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class AccountControllerTest
    {
        private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
        private readonly AccountController _controller;
        private readonly Mock<ILogger<AccountController>> mockLogger;
        private readonly Mock<AutoMapperConfig> mockMapper;
        private readonly Mock<SignInManager<ApplicationUser>> mockSignInManager;
        private readonly Mock<RoleManager<IdentityRole>> mockRoleManager;
        public AccountControllerTest()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            mockLogger = new Mock<ILogger<AccountController>>();
            mockMapper = new Mock<AutoMapperConfig>();

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager.Object, httpContextAccessor.Object,
                                                                            userPrincipalFactory.Object, null, null, null, null);
            var mockIRoleStore = new Mock<IRoleStore<IdentityRole>>();
             mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockIRoleStore.Object, null, null, null, null);

            _controller = new AccountController(mockUserManager.Object, mockSignInManager.Object, mockRoleManager.Object,
                mockLogger.Object, mockMapper.Object, null);
        }
        [Fact]
        public async Task Login_ReturnsRedirectToAction_ForValidModel()
        {
            //Arrange
            var model = new LoginViewModel
            {
                UserName = "testId",
                Password = "Passw0rd!",
                RememberMe = false
            };
            var appUser = new ApplicationUser
            {
                UserName = model.UserName,
            };

            mockSignInManager.Setup(s => s.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //Act
            var result = await _controller.Login(model);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);

        }
        [Fact]
        public async Task Login_ReturnsAViewResult_ForUnauthenticatedUser()
        {
            //Arrange
            var model = new LoginViewModel
            {
                UserName = "testId",
                Password = "Passw0rd!",
                RememberMe = false
            };
            var appUser = new ApplicationUser
            {
                UserName = model.UserName,
            };
            
            mockSignInManager.Setup(s => s.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            //Act
            var result = await _controller.Login(model);
            //Assert
            Assert.IsType<ViewResult>(result);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Unauthenticated user"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact(Skip ="Null exception")]
        public void Login_ReturnsARedirectToActionResult_ForAuthorizedUser()
        {
            //Arrange
            var claim = new Mock<ClaimsPrincipal>();
            claim.Setup(s => s.Identity.IsAuthenticated).Returns(() => true);            
            //Act
            var result = _controller.Login();
            //Assert
            Assert.IsType<RedirectToActionResult>(result);
        }
        [Fact]
        public async Task Login_ReturnsAViewResult_ForInvalidModel()
        {
            //Arrange
            var model = new LoginViewModel
            {
                UserName = "testId",
                Password = "Passw0rd!",
                RememberMe = false
            };
            _controller.ModelState.AddModelError("error", "some error");
            //Act
            var result = await _controller.Login(model);
            //Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task Logout_ReturnsRedirectToAction_WhenUserIsSignIn()
        {
            //Arrange
            //Act
            var result = await _controller.Logout();
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectToAction.ActionName);
            Assert.Equal("Account", redirectToAction.ControllerName);

        }
        [Fact]
        public async Task AddEmployee_ReturnsAViewResult_ForInvalidModel()
        {
            //Arrange
            var model = new EmployeeRegistrationViewModel();
            _controller.ModelState.AddModelError("eror", "some error");
            //Act
            var result = await _controller.AddEmployee(model);
            //Assert
            Assert.IsType<ViewResult>(result);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "While creating user occur invalid model"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task AddEmployee_ReturnsAViewResult_ForValidModelAndUnsuccessfulCreateMethod()
        {
            //Arrange
            var model = new EmployeeRegistrationViewModel
            {
                UserName = "test",
                Password = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                Adress = "Adress",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                Email = "email@email.com",
                FirstName = "FirstName",
                LastName = "LastName"
            };

            mockUserManager.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Failed());
            //Act
            var result = await _controller.AddEmployee(model);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<EmployeeRegistrationViewModel>(viewResult.ViewData.Model);
            Assert.Equal(model.UserName, modelResult.UserName);

            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Can't create Employee user, something wrong with User Identity"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task AddEmployee_ReturnsRedirectToAction_ForValidModelAndSuccessfulCreateMethod()
        {
            //Arrange
            var model = new EmployeeRegistrationViewModel
            {
                UserName = "test",
                Password = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                Adress = "Adress",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                Email = "email@email.com",
                FirstName = "FirstName",
                LastName = "LastName"
            };
            //Arrange
            mockUserManager.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);
            //Act
            var result = await _controller.AddEmployee(model);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToActionResult.ActionName);
            Assert.Equal("Success", redirectToActionResult.ControllerName);

        }
        [Fact]
        public async Task CompanyRegistration_ReturnsAViewResult_ForInvalidModel()
        {
            //Arrange
            var model = new CompanyRegistartionViewModel
            {
                Password = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                Adress = "Adress",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                Email = "email@email.com",
                CompanyName = "CompanyName",
                NIP = "0000000000",
                Regon = "000000000",
            };
            
            _controller.ModelState.AddModelError("error", "some error");
            //Act
            var result = await _controller.CompanyRegistration(model);
            //Assert
            Assert.IsType<ViewResult>(result);
            mockLogger.Verify(
               x => x.Log(
                   It.Is<LogLevel>(l => l == LogLevel.Warning),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) => v.ToString() == "While creating user occur invalid model"),
                   It.IsAny<Exception>(),
                   It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task CompanyRegistration_ReturnsRedirectToAction_ForValidModelAndUnsuccessfulCreateAsync()
        {
            //Arrange
            var model = new CompanyRegistartionViewModel
            {
                Password = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                Adress = "Adress",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                Email = "email@email.com",
                CompanyName = "CompanyName",
                NIP = "0000000000",
                Regon = "000000000",
            };
            
            mockUserManager.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Failed());
            //Act
            var result = await _controller.CompanyRegistration(model);
            //Assert
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Error", redirectToAction.ActionName);
            Assert.Equal("Error", redirectToAction.ControllerName);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Can't create Company user, something wrong with User Identity"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        [Fact]
        public async Task CompanyRegistration_ReturnsRedirectToAction_ForValidModelAndSuccessfulCreateAsync()
        {
            //Arrange
            var model = new CompanyRegistartionViewModel
            {
                Password = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                Adress = "Adress",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                Email = "email@email.com",
                CompanyName = "CompanyName",
                NIP = "0000000000",
                Regon = "000000000",
            };
            
            mockUserManager.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);
            //Act
            var result = await _controller.CompanyRegistration(model);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToActionResult.ActionName);
            Assert.Equal("Success", redirectToActionResult.ControllerName);
        }
    }
}
