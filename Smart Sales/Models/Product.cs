using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace Smart_Sales.Models
{
    public class Product 
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double JumlaPrice { get; set; }
        public string ProductUnit { get; set; }
        public double RetailPrice { get; set; }
        public double JumlaSoldQty { get; set; }
        public double RetailSoldQty { get; set; }
        public double TotalSales { get; set; }
        public double AvailableQuantity { get; set; }
        public double TotalQuantity { get; set; }
        public string ProductQty { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set;}
        public bool IsExpense {  get; set; }

    }
}
