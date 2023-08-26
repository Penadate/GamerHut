using BLL.Models.Category;
using BLL.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetProductsByCategoryIdAsync(int categoryId);
        Task<ProductModel> GetProductByIdAsync(int id);
        Task<ProductModel> CreateProductAsync(CreateProductModel createProductModel);
        Task UpdateAsync(int id, UpdateProductModel updateProductModel);
        Task DeleteAsync(int id, string password);
    }
}
