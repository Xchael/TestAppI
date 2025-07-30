using TestData.Models;

namespace TestAppI.Services.Interfaces
{
    public interface ITestAppService
    {
        Task<List<TestTable>> GetAllTestTableAsync();
    }
}
