using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestAppI.Controllers;
using TestAppI.Services.Interfaces;
using TestData.Models;
using Xunit;

namespace TestAppITests.TestApp.Controllers
{
    public class TestAppControllerTests
    {
        [Fact]
        public async Task GetAllTestApps_ReturnsOkResult_WithListOfTestTable()
        {
            // Arrange
            var mockService = new Mock<ITestAppService>();
            var testData = new List<TestTable>
            {
                new TestTable { Id = 1, Name = "Test1", IsEnabled = true, Data = "Data1" },
                new TestTable { Id = 2, Name = "Test2", IsEnabled = false, Data = "Data2" }
            };
            mockService.Setup(s => s.GetAllTestTableAsync()).ReturnsAsync(testData);
            var controller = new TestAppController(mockService.Object);

            // Act
            var result = await controller.GetAllTestApps();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TestTable>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("Test1", returnValue[0].Name);
        }

        [Fact]
        public async Task GetAllTestApps_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var mockService = new Mock<ITestAppService>();
            mockService.Setup(s => s.GetAllTestTableAsync()).ReturnsAsync(new List<TestTable>());
            var controller = new TestAppController(mockService.Object);

            // Act
            var result = await controller.GetAllTestApps();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<TestTable>>(okResult.Value);
            Assert.Empty(returnValue);
        }
    }
}
