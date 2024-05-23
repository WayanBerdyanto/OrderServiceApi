using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class Product
    {
            public int productID { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public decimal price { get; set; }
            public int categoryID { get; set; }
            public object categoryName { get; set; }
            public int quantity { get; set; }
        }
}