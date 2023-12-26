using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Smart_Sales.Models
{
    public class Production
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public  string ProductUnit { get; set; }
        public double ProductQuantity { get; set; }
        public DateTime ProductionDate { get; set; }

    }
}
