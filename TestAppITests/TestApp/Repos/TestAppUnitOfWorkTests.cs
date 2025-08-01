using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Data;
using TestData.Models;
using TestData.Repos;

namespace TestAppITests.TestApp.Repos
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly TestAppIDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<TestAppIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TestAppIDbContext(options);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public void Dispose_ShouldDisposeContext()
        {
            // Arrange
            var context = new Mock<TestAppIDbContext>();
            var unitOfWork = new UnitOfWork(context.Object);

            // Act
            unitOfWork.Dispose();

            // Assert
            context.Verify(c => c.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DisposeAsync_ShouldDisposeContextAsync()
        {
            // Arrange
            var context = new Mock<TestAppIDbContext>();
            var unitOfWork = new UnitOfWork(context.Object);

            // Act
            await unitOfWork.DisposeAsync();

            // Assert
            context.Verify(c => c.DisposeAsync(), Times.Once);
        }

        [Fact]
        public void Repository_ShouldReturnSameInstance()
        {
            // Arrange
            var repo1 = _unitOfWork.Repository<SomeEntity>();
            var repo2 = _unitOfWork.Repository<SomeEntity>();

            // Act & Assert
            Assert.Same(repo1.Read, repo2.Read);
            Assert.Same(repo1.Write, repo2.Write);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldReturnAffectedRows()
        {
            // Arrange
            var entity = new TestTable { Name = "Test" };
            _context.TestTable.Add(entity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _unitOfWork.SaveChangesAsync();

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void BeginTransaction_ShouldNotThrow()
        {
            // Act & Assert
            var exception = Record.Exception(() => _unitOfWork.BeginTransaction());
            Assert.Null(exception);
        }

        [Fact]
        public async Task CommitTransactionAsync_ShouldNotThrow()
        {
            // Arrange
            _unitOfWork.BeginTransaction();

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _unitOfWork.CommitTransactionAsync());
            Assert.Null(exception);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

    public class SomeEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
