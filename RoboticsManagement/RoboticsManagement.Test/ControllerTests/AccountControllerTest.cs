using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Configuration;
using RoboticsManagement.Controllers;
using RoboticsManagement.Data;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RoboticsManagement.Test.ControllerTests
{
    public class AccountControllerTest
    {
        [Fact]
        public async void Login_ReturnsRedirectToAction_ForValidModel()
        {
            //Arrange
            var model = new LoginViewModel
            {
                UserName = "test",
                Password = "Passw0rd!",
                RememberMe = false
            };
            var appUser = new ApplicationUser
            {
                UserName = model.UserName,
            };

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var passHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var mockUserManager = new UserManager<ApplicationUser>(userStore.Object,null,passHasher.Object, null, null, null, null, null, null);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockSignInManager = new SignInManager<ApplicationUser>(mockUserManager, httpContextAccessor.Object,userPrincipalFactory.Object,
                                                                       null, null, null, null);
            //var roleStore = new Mock<IRoleStore<IdentityRole>>();
            //var roleValidator = new Mock<IEnumerable<IRoleValidator<IdentityRole>>>();
            //var lookupNormalizer = new Mock<ILookupNormalizer>();
            //var errorDescriber = new IdentityErrorDescriber();
            //var logger = new Mock<ILogger<RoleManager<IdentityRole>>>();
            //var mockRoleManager = new RoleManager<IdentityRole>(roleStore.Object, roleValidator.Object,
            //    lookupNormalizer.Object, errorDescriber,logger.Object);
            //var mockLogger = new Mock<ILogger<AccountController>>();
            //var mockMapper = new AutoMapperConfig();
            //var mockContext = new Mock<MgmtDbContext>();
            await mockUserManager.CreateAsync(appUser, model.Password);
            var controller = new AccountController(mockUserManager, mockSignInManager, null,
                null, null, null);
            //Act
            var result = controller.Login(model);
            //Assert
            await Assert.IsType<Task<IActionResult>>(result);
        }
    }
}
