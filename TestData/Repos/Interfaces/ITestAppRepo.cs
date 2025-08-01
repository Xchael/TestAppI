using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Models;

namespace TestData.Repos.Interfaces
{
    public interface ITestAppRepo
    {
        /// <summary>
        /// Asynchronously retrieves a list of all test applications.
        /// This method is designed to be asynchronous to avoid blocking the calling thread,
        /// allowing for better performance in applications that require non-blocking I/O operations.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a list of TestTable objects.</returns>
        Task<List<TestTable>> GetAllTestAppsAsync(CancellationToken cancellationToken = default);
    }
}
