using System.Data.SqlClient;
using Dapper;
using OrderApi.DAL.Interface;
using OrderApi.Models;

namespace OrderApi.DAL
{
    public class OrderHeaderDAL : IOrderHeader
    {
        private readonly IConfiguration _config;
        public OrderHeaderDAL(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return @"Data Source=.\SQLEXPRESS;Initial Catalog=OrderDb;Integrated Security=True";
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public OrderHeader Insert(OrderHeader obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO OrderHeaders (CustomerId, OrderDate) VALUES (@CustomerId, @OrderDate); select @@IDENTITY";
                var param = new
                {
                    CustomerID = obj.CustomerID,
                    OrderDate = obj.OrderDate,
                };
                try
                {
                    var newId = conn.ExecuteScalar<int>(strSql, param);
                    obj.OrderHeaderID = newId;
                    return obj;
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

        public void Update(OrderHeader obj)
        {
            throw new NotImplementedException();
        }

        IEnumerable<OrderHeader> ICrud<OrderHeader>.GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM OrderHeaders ORDER BY OrderDate asc";
                var OrderHeaders = conn.Query<OrderHeader>(strSql);
                return OrderHeaders;
            }
        }

        public OrderHeader GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM OrderHeaders WHERE OrderHeaderId = @OrderHeaderId";
                var param = new { OrderHeaderId = id };
                var orderHeader = conn.QueryFirstOrDefault<OrderHeader>(strSql, param);
                if (orderHeader == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return orderHeader;
            }
        }
    }
}