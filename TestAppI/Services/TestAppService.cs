using TestAppI.Services.Interfaces;
using TestData.Models;
using TestData.Repos.Interfaces;

namespace TestAppI.Services
{
    public class TestAppService(ITestAppRepo _repo) : ITestAppService
    {
        // Constructor that initializes the service with a repository instance.
        // This allows the service to access data operations defined in the repository interface.


        // Asynchronously retrieves all entries from the TestTable.
        // The method signature includes a CancellationToken to allow the operation to be cancelled if needed,
        // which is important for long-running tasks to improve responsiveness.
        public async Task<List<TestTable>> GetAllTestTableAsync(CancellationToken cancellationToken = default)
        {
            return await _repo.GetAllTestAppsAsync(cancellationToken);
        }
    }
}
