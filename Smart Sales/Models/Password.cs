using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Smart_Sales.Models
{
    public class Password
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } 
        public TimeSpan LicenseTime { get; set; } 
    }
}
