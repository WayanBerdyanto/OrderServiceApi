using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.DTO
{
    public class UserUpdateBalance
    {
        public string UserName { get; set; }
        public decimal Balance { get; set; }
    }
}