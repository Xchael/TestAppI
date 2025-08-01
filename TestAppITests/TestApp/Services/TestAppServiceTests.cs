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
        private readonly Mock<ITestAppRepo> _mockRepo;
        private readonly TestAppService _service;

        public TestAppServiceTests()
        {
            _mockRepo = new Mock<ITestAppRepo>();
            _service = new TestAppService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllTestTableAsync_ReturnsListOfTestTable()
        {
            // Arrange
            var testData = new List<TestTable>
            {
                new TestTable { /* Initialize properties */ },
                new TestTable { /* Initialize properties */ }
            };
            _mockRepo.Setup(repo => repo.GetAllTestAppsAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(testData);

            // Act
            var result = await _service.GetAllTestTableAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllTestTableAsync_CallsRepoMethod()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllTestAppsAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new List<TestTable>());

            // Act
            await _service.GetAllTestTableAsync();

            // Assert
            _mockRepo.Verify(repo => repo.GetAllTestAppsAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
