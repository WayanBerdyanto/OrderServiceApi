using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.DTO;
using OrderApi.Models;

namespace OrderApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<User> GetUserByName(string username);
        Task UpdateUserBalance(UserUpdateBalance userUpdateBalance);
    }
}