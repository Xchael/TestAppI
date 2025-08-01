using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TestAppI.Controllers;
using TestAppI.Services.Interfaces;
using TestData.Models;
using Xunit;

namespace TestAppITests.TestApp.Controllers
{
    public class TestAppControllerTests
    {
        private readonly Mock<ITestAppService> _mockService;
        private readonly TestAppController _controller;

        public TestAppControllerTests()
        {
            _mockService = new Mock<ITestAppService>();
            _controller = new TestAppController(_mockService.Object, null);
        }

        [Fact]
        public async Task GetAllTestApps_ReturnsOkResult_WithTestApps()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var testApps = new List<TestTable> { new TestTable(), new TestTable() }; // Assuming TestApp is a valid model
            _mockService.Setup(service => service.GetAllTestTableAsync(cancellationToken)).ReturnsAsync(testApps);

            // Act
            var result = await _controller.GetAllTestApps(cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<List<TestTable>>(okResult.Value);
            Assert.Equal(testApps.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetAllTestApps_ReturnsEmptyList_WhenNoTestApps()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var testApps = new List<TestTable>();
            _mockService.Setup(service => service.GetAllTestTableAsync(cancellationToken)).ReturnsAsync(testApps);

            // Act
            var result = await _controller.GetAllTestApps(cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<List<TestTable>>(okResult.Value);
            Assert.Empty(returnValue);
        }
    }
}
