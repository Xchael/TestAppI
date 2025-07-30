using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppI.Services;
using TestData.Models;
using TestData.Repos.Interfaces;

namespace TestAppITests.TestApp.Services
{
    public class TestAppServiceTests
    {
        [Fact]
        public async Task GetAllTestTableAsync_ReturnsListOfTestTable()
        {
            // Arrange
            var testData = new List<TestTable>
            {
                new TestTable { Id = 1, Name = "Test1", IsEnabled = true, Data = "Data1" },
                new TestTable { Id = 2, Name = "Test2", IsEnabled = false, Data = "Data2" }
            };

            var repoMock = new Mock<ITestAppRepo>();
            repoMock.Setup(r => r.GetAllTestAppsAsync()).ReturnsAsync(testData);

            var service = new TestAppService(repoMock.Object);

            // Act
            var result = await service.GetAllTestTableAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Test1", result[0].Name);
            Assert.Equal("Test2", result[1].Name);
        }

        [Fact]
        public async Task GetAllTestTableAsync_ReturnsEmptyList_WhenNoData()
        {
            // Arrange
            var repoMock = new Mock<ITestAppRepo>();
            repoMock.Setup(r => r.GetAllTestAppsAsync()).ReturnsAsync(new List<TestTable>());

            var service = new TestAppService(repoMock.Object);

            // Act
            var result = await service.GetAllTestTableAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
