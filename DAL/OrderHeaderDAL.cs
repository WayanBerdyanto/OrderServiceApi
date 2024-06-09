using System.Data.SqlClient;
using Dapper;
using OrderApi.DAL.Interface;
using OrderApi.Models;

namespace OrderApi.DAL
{
    public class OrderHeaderDAL : IOrderHeader
    {
        private readonly IConfiguration _config;
        private readonly Connect _conn;
        public OrderHeaderDAL(IConfiguration config)
        {
            _config = config;
            _conn = new Connect(_config);
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(OrderHeader obj)
        {
            using (SqlConnection conn = _conn.GetConnectDb())
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
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"UPDATE OrderHeaders SET UserName = @UserName, OrderDate = @OrderDate";
                var param = new { UserName = obj.UserName, OrderDate = obj.OrderDate };
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
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"SELECT * FROM OrderHeaders ORDER BY OrderDate DESC";
                var OrderHeaders = conn.Query<OrderHeader>(strSql);
                return OrderHeaders;
            }
        }

        public OrderHeader GetById(int id)
        {
            using (SqlConnection conn = _conn.GetConnectDb())
            {
                var strSql = @"SELECT * FROM OrderHeaders WHERE OrderHeaderId = @OrderHeaderId";
                var param = new { OrderHeaderId = id };
                try
                {
                    var orderHeader = conn.QueryFirstOrDefault<OrderHeader>(strSql, param);
                    return orderHeader;

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