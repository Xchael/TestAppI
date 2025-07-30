using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Models;
using TestData.Repos.Interfaces;

namespace TestData.Repos
{
    public class TestAppRepo(IUnitOfWork unitOfWork) : ITestAppRepo
    {
        public async Task<List<TestTable>> GetAllTestAppsAsync()
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            return await unitOfWork.Repository<TestTable>().Read.GetAllAsync();
        }
    }
}
