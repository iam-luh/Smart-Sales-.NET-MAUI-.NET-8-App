using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart_Sales.Models;
using SQLite;

namespace Smart_Sales.Services
{
    public class UsernameService : IUsernameService
    {
        public UsernameService() 
        { 
            SetUpDb();
        }
        private SQLiteAsyncConnection conn;

        public void SetUpDb()
        {

            if (conn is null)
            {
                string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Usernames.db3");
                conn = new SQLiteAsyncConnection(dbpath);
                conn.CreateTableAsync<Username>();
            }
        }
        public Task<int> AddUsername(Username task)
        {
            SetUpDb();
            return conn.InsertAsync(task);
        }

        public Task<int> DeleteUsername(Username task)
        {
            SetUpDb();
            return conn.DeleteAsync(task);
        }

        public Task<List<Username>> GetAllUsernames()
        {
            SetUpDb();
            var nameslist = conn.Table<Username>().ToListAsync();
            return nameslist;
        }

        public Task<int> UpdateUsername(Username task)
        {
            SetUpDb();
            return conn.UpdateAsync(task);
        }
    }
}
