using BLL.Models.Category;
using BLL.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetCategoriesByGameIdAsync(int gameId);
        Task<CategoryModel> CreateCategoryAsync(CreateCategoryModel createCategoryModel);
        Task UpdateAsync(int id, UpdateCategoryModel updateCategoryModel);
        Task DeleteAsync(int id);
    }
}
