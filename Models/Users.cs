using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class Users
    {
        public string userName { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public decimal Balance { get; set; }
    }
}