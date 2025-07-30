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
    public class UnitOfWorkTests
    {
        private readonly Mock<TestAppIDbContext> _mockContext;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _mockContext = new Mock<TestAppIDbContext>();
            _unitOfWork = new UnitOfWork(_mockContext.Object);
        }


        [Fact]
        public void Dispose_CallsContextDispose()
        {
            _unitOfWork.Dispose();
            _mockContext.Verify(c => c.Dispose(), Times.Once);
        }

        [Fact]
        public async Task DisposeAsync_CallsContextDisposeAsync()
        {
            await _unitOfWork.DisposeAsync();
            _mockContext.Verify(c => c.DisposeAsync(), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_CallsContextSaveChangesAsync()
        {
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            var result = await _unitOfWork.SaveChangesAsync();
            Assert.Equal(1, result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public void SaveChanges_CallsContextSaveChanges()
        {
            _mockContext.Setup(c => c.SaveChanges()).Returns(2);
            var result = _unitOfWork.SaveChanges();
            Assert.Equal(2, result);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void BeginTransaction_CallsContextDatabaseBeginTransaction()
        {
            var mockDatabase = new Mock<DatabaseFacade>(_mockContext.Object);
            _mockContext.SetupGet(c => c.Database).Returns(mockDatabase.Object);

            _unitOfWork.BeginTransaction();

            mockDatabase.Verify(d => d.BeginTransaction(), Times.Once);
        }

        [Fact]
        public async Task BeginTransactionAsync_CallsContextDatabaseBeginTransactionAsync()
        {
            var mockDatabase = new Mock<DatabaseFacade>(_mockContext.Object);
            _mockContext.SetupGet(c => c.Database).Returns(mockDatabase.Object);

            await _unitOfWork.BeginTransactionAsync();

            mockDatabase.Verify(d => d.BeginTransactionAsync(default), Times.Once);
        }

        [Fact]
        public void CommitTransaction_CallsContextDatabaseCommitTransaction()
        {
            var mockDatabase = new Mock<DatabaseFacade>(_mockContext.Object);
            _mockContext.SetupGet(c => c.Database).Returns(mockDatabase.Object);

            _unitOfWork.CommitTransaction();

            mockDatabase.Verify(d => d.CommitTransaction(), Times.Once);
        }

        [Fact]
        public async Task CommitTransactionAsync_CallsContextDatabaseCommitTransactionAsync()
        {
            var mockDatabase = new Mock<DatabaseFacade>(_mockContext.Object);
            _mockContext.SetupGet(c => c.Database).Returns(mockDatabase.Object);

            await _unitOfWork.CommitTransactionAsync();

            mockDatabase.Verify(d => d.CommitTransactionAsync(default), Times.Once);
        }

        [Fact]
        public void RollbackTransaction_CallsContextDatabaseRollbackTransaction()
        {
            var mockDatabase = new Mock<DatabaseFacade>(_mockContext.Object);
            _mockContext.SetupGet(c => c.Database).Returns(mockDatabase.Object);

            _unitOfWork.RollbackTransaction();

            mockDatabase.Verify(d => d.RollbackTransaction(), Times.Once);
        }

        [Fact]
        public async Task RollbackTransactionAsync_CallsContextDatabaseRollbackTransactionAsync()
        {
            var mockDatabase = new Mock<DatabaseFacade>(_mockContext.Object);
            _mockContext.SetupGet(c => c.Database).Returns(mockDatabase.Object);

            await _unitOfWork.RollbackTransactionAsync();

            mockDatabase.Verify(d => d.RollbackTransactionAsync(default), Times.Once);
        }
    }
}
