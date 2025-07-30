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
        Task<List<TestTable>> GetAllTestAppsAsync();
    }
}
