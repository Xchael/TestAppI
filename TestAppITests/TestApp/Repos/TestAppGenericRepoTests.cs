using Microsoft.EntityFrameworkCore;
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
    public class GenericRepoTests
    {
        private readonly Mock<DbSet<TestTable>> _mockSet;
        private readonly Mock<TestAppIDbContext> _mockContext;
        private readonly GenericRepo<TestTable> _repo;

        public GenericRepoTests()
        {
            _mockSet = new Mock<DbSet<TestTable>>();
            _mockContext = new Mock<TestAppIDbContext>();
            _mockContext.Setup(m => m.Set<TestTable>()).Returns(_mockSet.Object);
            _repo = new GenericRepo<TestTable>(_mockContext.Object);
        }

        [Fact]
        public void Add_CallsAddOnDbSet()
        {
            var entity = new TestTable { Id = 1, Name = "Test" };
            _repo.Add(entity);
            _mockSet.Verify(m => m.Add(entity), Times.Once);
        }

        [Fact]
        public async Task AddAsync_CallsAddAsyncOnDbSet()
        {
            var entity = new TestTable { Id = 2, Name = "Async" };
            await _repo.AddAsync(entity);
            _mockSet.Verify(m => m.AddAsync(entity, default), Times.Once);
        }

        [Fact]
        public void AddRange_CallsAddRangeOnDbSet()
        {
            var entities = new List<TestTable> { new() { Id = 1 }, new() { Id = 2 } };
            _repo.AddRange(entities);
            _mockSet.Verify(m => m.AddRange(entities), Times.Once);
        }

        [Fact]
        public async Task AddRangeAsync_CallsAddRangeAsyncOnDbSet()
        {
            var entities = new List<TestTable> { new() { Id = 1 }, new() { Id = 2 } };
            await _repo.AddRangeAsync(entities);
            _mockSet.Verify(m => m.AddRangeAsync(entities, default), Times.Once);
        }

        [Fact]
        public void Update_CallsUpdateOnDbSet()
        {
            var entity = new TestTable { Id = 1 };
            _repo.Update(entity);
            _mockSet.Verify(m => m.Update(entity), Times.Once);
        }

        [Fact]
        public void UpdateRange_CallsUpdateRangeOnDbSet()
        {
            var entities = new List<TestTable> { new() { Id = 1 }, new() { Id = 2 } };
            _repo.UpdateRange(entities);
            _mockSet.Verify(m => m.UpdateRange(entities), Times.Once);
        }

        [Fact]
        public void Remove_CallsRemoveOnDbSet()
        {
            var entity = new TestTable { Id = 1 };
            _repo.Remove(entity);
            _mockSet.Verify(m => m.Remove(entity), Times.Once);
        }

        [Fact]
        public void RemoveRange_CallsRemoveRangeOnDbSet()
        {
            var entities = new List<TestTable> { new() { Id = 1 }, new() { Id = 2 } };
            _repo.RemoveRange(entities);
            _mockSet.Verify(m => m.RemoveRange(entities), Times.Once);
        }

        [Fact]
        public void GetById_ReturnsEntity_WhenFound()
        {
            var entity = new TestTable { Id = 1 };
            _mockSet.Setup(m => m.Find(1)).Returns(entity);
            var result = _repo.GetById(1);
            Assert.Equal(entity, result);
        }

        [Fact]
        public void GetById_Throws_WhenNotFound()
        {
            _mockSet.Setup(m => m.Find(1)).Returns((TestTable)null);
            Assert.Throws<KeyNotFoundException>(() => _repo.GetById(1));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenFound()
        {
            var entity = new TestTable { Id = 1 };
            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(entity);
            var result = await _repo.GetByIdAsync(1);
            Assert.Equal(entity, result);
        }

        [Fact]
        public async Task GetByIdAsync_Throws_WhenNotFound()
        {
            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync((TestTable)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.GetByIdAsync(1));
        }
    }
}
