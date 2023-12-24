using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Smart_Sales.Models
{
   public class Invoice
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }  
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public string Category { get; set; }
        public double Cost { get; set; }
        public double ProductCost { get; set; }
        public double Quantity { get; set; }
        public string ProductUnit { get; set; }
        public string Name { get; set; }

        public bool IsPaid { get; set; }
        public bool IsNotPaid { get; set; }
        public bool IsExpense { get; set; }
        public bool CantEdit { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public TimeSpan CreatedTime { get; set; }


    }
}
