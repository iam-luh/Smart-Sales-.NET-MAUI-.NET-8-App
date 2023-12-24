using Smart_Sales.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Sales.Services
{
    public class StoreService : IStoreService
    {
        public StoreService()
        {
            SetUpDb();
        }
        private SQLiteAsyncConnection conn;

        public void SetUpDb()
        {

            if (conn is null)
            {
                string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Productions.db3");
                conn = new SQLiteAsyncConnection(dbpath);
                conn.CreateTableAsync<Production>();
            }
        }
        public Task<int> AddProduction(Production task)
        {
            SetUpDb();
            return conn.InsertAsync(task);
        }

        public Task<int> DeleteProduction(Production task)
        {
            SetUpDb();
            return conn.DeleteAsync(task);
        }

        public Task<List<Production>> GetAllProductions()
        {
            SetUpDb();
            var productionlist = conn.Table<Production>().ToListAsync();
            return productionlist;
        }

        public Task<int> UpdateProduction(Production task)
        {
            SetUpDb();
            return conn.UpdateAsync(task);
        }
    }
}
