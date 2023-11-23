using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart_Sales.Models;

namespace Smart_Sales.Services
{
    public interface IInvoiceService
    {
        Task<List<Invoice>> GetAllInvoices();        
        Task<int> AddInvoice(Invoice task);
        Task<int> DeleteInvoice(Invoice task);
        Task<int> UpdateInvoice(Invoice task);

    }
}
