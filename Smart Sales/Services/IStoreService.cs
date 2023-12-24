using Smart_Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Sales.Services
{
    public interface IStoreService
    {
        Task<List<Production>> GetAllProductions();
        Task<int> AddProduction(Production task);
        Task<int> DeleteProduction(Production task);
        Task<int> UpdateProduction(Production task);
    }
}
