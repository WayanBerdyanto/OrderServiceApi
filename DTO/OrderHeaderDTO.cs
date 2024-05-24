using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.DTO
{
    public class OrderHeaderDTO
    {
        public int OrderHeaderID { get; set; }
        public string username { get; set; }

        public DateTime OrderDate { get; set; }
    }
}