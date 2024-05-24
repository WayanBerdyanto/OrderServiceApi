using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderApi.DTO;
using OrderApi.Models;

namespace OrderApi.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5031");
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            var response = await _httpClient.GetAsync("/users");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<IEnumerable<User>>(results);
                if (user == null)
                {
                    throw new ArgumentException("Cannot get users");
                }
                return user;
            }
            else
            {
                throw new ArgumentException($"Cannot get User - httpstatus: {response.StatusCode}");
            }
        }

        public async Task<User> GetProductByName(string username)
        {
            var response = await _httpClient.GetAsync($"/users/{username}");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(results);
                if (user == null)
                {
                    throw new ArgumentException("Cannot get User");
                }
                return user;
            }
            else
            {
                throw new ArgumentException($"Cannot get User - httpstatus: {response.StatusCode}");
            }
        }

        public async Task UpdateUserBalance(UserUpdateBalance userUpdateBalance)
        {
            var json = JsonSerializer.Serialize(userUpdateBalance);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/users/updatebalance", data);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Cannot update product stock - httpstatus: {response.StatusCode}");
            }
        }
    }
}