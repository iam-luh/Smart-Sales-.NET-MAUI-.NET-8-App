using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Smart_Sales.Models;

namespace Smart_Sales.Services
{
    public class InvoiceService : IInvoiceService
    {
        private SQLiteAsyncConnection conn;
        //public InvoiceService()
        //{ 
        //    SetUpDb();
        //}

        public async void SetUpDb()
        {            
            if (conn is null)
            {   string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Invoices.db3");
                conn = new SQLiteAsyncConnection(dbpath);
                await conn.CreateTableAsync<Invoice>();
            }           
        }

        public Task<int> AddInvoice(Invoice task)
        {
            SetUpDb();
            return conn.InsertAsync(task);
        }

        public Task<int> DeleteInvoice(Invoice task)
        {
            SetUpDb();
            return conn.DeleteAsync(task);           
        }

        public Task<List<Invoice>> GetAllInvoices()
        {
            SetUpDb();
            var studentList= conn.Table<Invoice>().ToListAsync();
            return studentList;
        }

        public Task<int> UpdateInvoice(Invoice task)
        {
            SetUpDb();
            return conn.UpdateAsync(task);
        }

        
       
    }
}
