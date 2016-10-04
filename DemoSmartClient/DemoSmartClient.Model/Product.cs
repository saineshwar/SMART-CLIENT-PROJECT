using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoSmartClient.Model
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductNumber { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string ProductClass { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CLientIDToken { get; set; }
    }
}
