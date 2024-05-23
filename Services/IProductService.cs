using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderApi.DAL;
using OrderApi.Models;

namespace OrderApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task UpdateProductStock(ProductUpdateStockDto productUpdateStockDto);
    }
}