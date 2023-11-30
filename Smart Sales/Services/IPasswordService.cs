using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart_Sales.Models;

namespace Smart_Sales.Services
{
    public interface IPasswordService
    {
        Task<List<Password>> GetAllPasswords();        
        Task<int> AddPassword(Password task);
        Task<int> DeletePassword(Password task);
        Task<int> UpdatePassword(Password task);

    }
}
