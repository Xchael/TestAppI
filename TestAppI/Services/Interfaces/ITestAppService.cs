using TestData.Models;

namespace TestAppI.Services.Interfaces
{
    public interface ITestAppService
    {
        // Asynchronously retrieves a list of all TestTable entries.
        // The method returns a Task that resolves to a List of TestTable objects.
        // The CancellationToken allows the operation to be cancelled if needed, providing better control over long-running tasks.
        Task<List<TestTable>> GetAllTestTableAsync(CancellationToken cancellationToken = default);
    }
}
