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
using System.Threading.Tasks;
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class AccountControllerTest
    {
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

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager, httpContextAccessor.Object,
                                                                            userPrincipalFactory.Object, null, null, null, null);
            mockSignInManager.Setup(s => s.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new AccountController(mockUserManager, mockSignInManager.Object, null,
                null, null, null);

            //Act
            var result = await controller.Login(model);
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

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager, httpContextAccessor.Object,
                                                                            userPrincipalFactory.Object, null, null, null, null);
            mockSignInManager.Setup(s => s.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var logger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(mockUserManager, mockSignInManager.Object, null,
                logger.Object, null, null);

            //Act
            var result = await controller.Login(model);
            //Assert
            Assert.IsType<ViewResult>(result);
            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Unauthenticated user"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
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
            var controller = new AccountController(null, null, null,
                                                   null, null, null);
            controller.ModelState.AddModelError("error", "some error");

            //Act
            var result = await controller.Login(model);
            //Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task Logout_ReturnsRedirectToAction_WhenUserIsSignIn()
        {
            //Arrange

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager, httpContextAccessor.Object,
                                                                            userPrincipalFactory.Object, null, null, null, null);
            var logger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(mockUserManager, mockSignInManager.Object, null,
                logger.Object, null, null);

            //Act
            var result = await controller.Logout();
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

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager, httpContextAccessor.Object,
                                                                            userPrincipalFactory.Object, null, null, null, null);

            var logger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(mockUserManager, mockSignInManager.Object, null,
                logger.Object, null, null);
            controller.ModelState.AddModelError("eror", "some error");
            //Act
            var result = await controller.AddEmployee(model);
            //Assert
            Assert.IsType<ViewResult>(result);
            logger.Verify(
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
            var userStore = new Mock<IUserStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object,null, null, null, null, null, null, null, null);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager, httpContextAccessor.Object,
                                                                            userPrincipalFactory.Object, null, null, null, null);
            var mockMapper = new Mock<AutoMapperConfig>();

            var logger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(mockUserManager.Object, null, null,
                logger.Object, mockMapper.Object, null);

            mockUserManager.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(),model.Password))
                .ReturnsAsync(IdentityResult.Failed());
            //Act
            var result = await controller.AddEmployee(model);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsType<EmployeeRegistrationViewModel>(viewResult.ViewData.Model);
            Assert.Equal(model.UserName, modelResult.UserName);

            logger.Verify(
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
            var userStore = new Mock<IUserStore<ApplicationUser>>();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager, httpContextAccessor.Object,
                                                                            userPrincipalFactory.Object, null, null, null, null);
            var mockMapper = new Mock<AutoMapperConfig>();
            var mockIRoleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockIRoleStore.Object, null, null, null, null);

            var logger = new Mock<ILogger<AccountController>>();
            var controller = new AccountController(mockUserManager.Object, null, mockRoleManager.Object,
                logger.Object, mockMapper.Object, null);

            mockUserManager.Setup(s => s.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);
            //Act
            var result = await controller.AddEmployee(model);
            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Success", redirectToActionResult.ActionName);
            Assert.Equal("Success", redirectToActionResult.ControllerName);

        }
    }
}
