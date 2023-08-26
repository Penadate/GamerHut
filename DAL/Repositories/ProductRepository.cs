using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Category).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Product?>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _dbSet.Include(c => c.Category).Where(c => c.CategoryId == categoryId).ToListAsync();
        }
    }
}
