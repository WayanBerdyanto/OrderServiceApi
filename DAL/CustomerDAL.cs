using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using OrderApi.DAL.Interface;
using OrderApi.Models;

namespace OrderApi.DAL
{
    public class CustomerDAL : ICustomer
    {
        private readonly IConfiguration _config;
        public CustomerDAL(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return @"Data Source=.\SQLEXPRESS;Initial Catalog=OrderDb;Integrated Security=True";
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Customer WHERE CustomerId = @CustomerId";
                try
                {
                    conn.Execute(strSql, new { CustomerId = id });
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Customers ORDER BY CustomerName asc";
                var Customers = conn.Query<Customer>(strSql);
                return Customers;
            }
        }

        public Customer GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Customers  WHERE CustomerID = @CustomerID";
                var param = new { CustomerID = id };
                var customer = conn.QuerySingleOrDefault<Customer>(strSql, param);
                if (customer == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return customer;
            }
        }

        public void Insert(Customer obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Customers (CustomerName) VALUES (@CustomerName); select @@IDENTITY";
                var param =  new { CustomerName = obj.CustomerName };
                try
                {
                    conn.Execute(strSql, param);
                }   
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public void Update(Customer obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Customer SET CustomerName = @CustomerName WHERE CustomerId=@customerId";
                var param = new
                {
                    CustomerName = obj.CustomerName,
                    CustomerId = obj.CustomerID
                };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }

            }

        }
    }
}