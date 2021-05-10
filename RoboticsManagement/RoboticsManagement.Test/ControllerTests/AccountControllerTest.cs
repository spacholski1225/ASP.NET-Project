using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
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
    }
}
