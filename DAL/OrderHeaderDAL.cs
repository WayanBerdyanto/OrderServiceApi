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

        public void Insert(OrderHeader obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO OrderHeaders (UserName, OrderDate) VALUES (@UserName, @OrderDate); select @@IDENTITY";
                var param = new
                {
                    UserName = obj.UserName,
                    OrderDate = obj.OrderDate,
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

        public void Update(OrderHeader obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE OrderHeaders SET UserName = @UserName, OrderDate = @OrderDate";
                var param = new { UserName = obj.UserName, OrderDate = obj.OrderDate};
                try
                {
                    var newId = conn.ExecuteScalar<string>(strSql, param);
                    obj.UserName = newId;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }
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