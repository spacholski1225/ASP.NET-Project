using Microsoft.AspNetCore.Mvc;
using RoboticsManagement.Controllers;
using System;
using Xunit;

namespace RoboticsManagement.Test
{
    public class ErrorControllerTest
    {
        [Fact]
        public void HttpStatusCodeHandler_Returns_The_Correct_View()
        {
            // Arrange
            int statusCode = 404;
            var controller = new ErrorController();
            // Act
            var actionResult = controller.HttpStatusCodeHandler(statusCode);
            var viewResult = new ViewResult();
            // Assert
            var result = actionResult;
            Assert.IsType(viewResult.GetType(), result);
        }
    }
}
