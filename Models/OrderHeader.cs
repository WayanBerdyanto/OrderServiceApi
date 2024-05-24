using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class OrderHeader
    {
        public int OrderHeaderID { get; set; }
        public string? UserName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}