using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart_Sales.Models;

namespace Smart_Sales.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();        
        Task<int> AddProduct(Product task);
        Task<int> DeleteProduct(Product task);
        Task<int> UpdateProduct(Product task);
    }
}
