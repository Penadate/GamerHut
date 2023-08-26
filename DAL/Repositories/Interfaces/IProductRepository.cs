using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product?>> GetProductsByCategoryIdAsync(int categoryId);
    }
}
