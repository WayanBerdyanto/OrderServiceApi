using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.DTO
{
    public class OrderHeaderCreateDTO
    {
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}