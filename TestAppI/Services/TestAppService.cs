using TestAppI.Services.Interfaces;
using TestData.Models;
using TestData.Repos.Interfaces;

namespace TestAppI.Services
{
    public class TestAppService(ITestAppRepo _repo) : ITestAppService
    {
        public async Task<List<TestTable>> GetAllTestTableAsync()
        {
            return await _repo.GetAllTestAppsAsync();
        }
    }
}
