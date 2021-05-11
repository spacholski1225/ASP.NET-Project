using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RoboticsManagement.Controllers;
using System;
using Xunit;

namespace RoboticsManagement.Test
{
    public class ErrorControllerTest
    {
        [Fact]
        public void HttpStatusCodeHandler_ReturnsViewResultAndCorrectLogMessage_ForStatusCode()
        {
            // Arrange
            int statusCode = 404;
            var mockLogger = new Mock<ILogger<ErrorController>>();
            var controller = new ErrorController(mockLogger.Object);
            // Act
            var result = controller.HttpStatusCodeHandler(statusCode);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("NotFound", viewResult.ViewName);
            
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Can't find page 404"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
    }
}
