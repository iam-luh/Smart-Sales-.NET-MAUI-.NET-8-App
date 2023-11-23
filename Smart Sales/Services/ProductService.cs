using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Smart_Sales.Models;

namespace Smart_Sales.Services
{
    public class ProductService : IProductService
    {
        private SQLiteAsyncConnection conn;
        //public ProductService()
        //{ 
        //    SetUpDb();
        //}

        public async void SetUpDb()
        {
            SetUpDb();
            if (conn is null)
            {   string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Products.db3");
                conn = new SQLiteAsyncConnection(dbpath);
                await conn.CreateTableAsync<Product>();
            }           
        }

        public Task<int> AddProduct(Product task)
        {
            SetUpDb();
            return conn.InsertAsync(task);
        }

        public Task<int> DeleteProduct(Product task)
        {
            SetUpDb();
            return conn.DeleteAsync(task);
        }

        public Task<List<Product>> GetAllProducts()
        {
            SetUpDb();
            var productList= conn.Table<Product>().ToListAsync();
            return productList;
        }

        public Task<int> UpdateProduct(Product task)
        {
            SetUpDb();
            return conn.UpdateAsync(task);
        }

       
    }
}
