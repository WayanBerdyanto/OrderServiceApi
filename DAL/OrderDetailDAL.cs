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
    public class OrderDetailDAL : IOrderDetail
    {
        private readonly IConfiguration _config;
        public OrderDetailDAL(IConfiguration config)
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

        IEnumerable<OrderDetail> ICrud<OrderDetail>.GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM OrderDetails  ORDER BY OrderHeaderId asc";
                var OrderDetail = conn.Query<OrderDetail>(strSql);
                return OrderDetail;
            }
        }

        public OrderDetail GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM OrderDetails WHERE OrderDetailId = @OrderDetailId";
                    var OrderDetail = conn.QuerySingleOrDefault<OrderDetail>(strSql, new {OrderDetailId = id});
                    if(OrderDetail == null)
                    {
                        throw new ArgumentException("Data tidak ditemukan");
                    }
                        return OrderDetail;
            }
        }

        public OrderDetail Insert(OrderDetail obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO OrderDetails (OrderHeaderId, ProductId, Quantity, Price) VALUES (@OrderHeaderId, @ProductId, @Quantity, @Price); select @@IDENTITY";
                var param = new {OrderHeaderId = obj.OrderHeaderId, ProductId = obj.ProductId, Quantity = obj.Quantity, Price = obj.Price};
                try
                {
                    var newId = conn.ExecuteScalar<int>(strSql, param);
                    obj.OrderDetailId = newId;
                    return obj;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }
        }

        public void Update(OrderDetail obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE OrderDetails SET OrderHeaderId = @OrderHeaderId, ProductId = @ProductId, Quantity = @Quantity, Price = @Price";
                var param = new {OrderHeaderId = obj.OrderHeaderId, ProductId = obj.ProductId, Quantity = obj.Quantity, Price = obj.Price};
                try
                {
                    var newId = conn.ExecuteScalar<int>(strSql, param);
                    obj.OrderDetailId = newId;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }
        }
    }
}