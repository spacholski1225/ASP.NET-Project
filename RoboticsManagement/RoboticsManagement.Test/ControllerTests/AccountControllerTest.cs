using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Controllers;
using RoboticsManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RoboticsManagement.Test.ControllerTests
{
    public class AccountControllerTest
    {
        private readonly AccountController _sut;
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly Mock<IPrincipal> mockPrincipal;
        private readonly Mock<HttpContext> mockHttpContext;


        public AccountControllerTest()
        {

            var user = new ApplicationUser
            {
                UserName = "test",
                Id = "1"
            };

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("name", user.UserName)
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);
            
            //finish it
        }
        public Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
