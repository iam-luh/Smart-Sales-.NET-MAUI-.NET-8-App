using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart_Sales.Models;

namespace Smart_Sales.Services
{
    public interface IUsernameService
    {
        Task<List<Username>> GetAllUsernames();        
        Task<int> AddUsername(Username task);
        Task<int> DeleteUsername(Username task);
        Task<int> UpdateUsername(Username task);

    }
}
