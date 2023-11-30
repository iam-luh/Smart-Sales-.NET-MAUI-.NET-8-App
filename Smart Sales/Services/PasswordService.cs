using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Smart_Sales.Models;

namespace Smart_Sales.Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordService()
        {
            SetUpDb();
        }
        private SQLiteAsyncConnection conn;
        

        public  void SetUpDb()
        {            
            if (conn is null)
            {   string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Passwords.db3");
                conn = new SQLiteAsyncConnection(dbpath);
                 conn.CreateTableAsync<Password>(); 
            }           
        }

        public  Task<int> AddPassword(Password pass)
        {
             SetUpDb();
            return  conn.InsertAsync(pass);
        }

        public  Task<int> DeletePassword(Password pass)
        {
             SetUpDb();
            return  conn.DeleteAsync(pass);           
        }

        public  Task<List<Password>> GetAllPasswords()
        {
            SetUpDb();
            var studentList= conn.Table<Password>().ToListAsync();
            return  studentList;
        }

        public  Task<int> UpdatePassword(Password task)
        {
            SetUpDb();
            return conn.UpdateAsync(task);
        }

        
       
    }
}
