using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Models;
using TestData.Repos.Interfaces;

namespace TestData.Repos
{
    // The TestAppRepo class implements the ITestAppRepo interface and is responsible for data access related to TestTable entities.
    public class TestAppRepo(IUnitOfWork unitOfWork) : ITestAppRepo
    {
        // Constructor that initializes the repository with a unit of work.
        // The unitOfWork parameter is required to ensure that the repository can perform operations on the database.

        // Asynchronously retrieves all TestTable entities from the database.
        // The method returns a list of TestTable objects and accepts a CancellationToken to allow for cancellation of the operation.
        public async Task<List<TestTable>> GetAllTestAppsAsync(CancellationToken cancellationToken = default)
        {
            // Check if the unitOfWork is null to prevent null reference exceptions.
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            // Calls the repository's Read method to get all TestTable entities asynchronously.
            return await unitOfWork.Repository<TestTable>().Read.GetAllAsync(cancellationToken);
        }
    }
}
