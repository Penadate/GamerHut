using AutoMapper;
using BLL.Exceptions;
using BLL.Models.Category;
using BLL.Models.Product;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductService(
            AppDbContext dbContext,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<ProductModel> CreateProductAsync(CreateProductModel createProductModel)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(createProductModel.CategoryId)
                ?? throw new NotFoundException($"Category with id \"{createProductModel.CategoryId}\" was not found!");

            var product = new Product();

            product.Name = createProductModel.Name;
            product.Category = category;
            product.CategoryId = category.Id;
            product.Price = createProductModel.Price;
            product.Count = createProductModel.Count;
            product.Date = DateTime.Today;
            product.Nickname = createProductModel.Nickname;
            product.Password = createProductModel.Password;
            product.Contact = createProductModel.Contact;

            await _productRepository.CreateAsync(product);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProductModel>(product);
        }

        public async Task<IEnumerable<ProductModel>> GetProductsByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                throw new BadRequestException($"Invalid id value: {categoryId}");
            }
            var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);

            if (products == null)
            {
                throw new NotFoundException($"Products with category id {categoryId} was not found!");
            }
            return _mapper.Map<IEnumerable<ProductModel>>(products);
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException($"Invalid id value: {id}");
            }

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} was not found!");
            }

            return _mapper.Map<ProductModel>(product);
        }
        public async Task DeleteAsync(int id, string password)
        {
            if (id <= 0)
            {
                throw new BadRequestException($"Invalid id value: {id}");
            }

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} was not found!");
            }

            if (product.Password != password)
            {
                throw new BadRequestException("Password is inccorect!");
            }

            _productRepository.Delete(product);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(int id, UpdateProductModel updateProductModel)
        {
            if (id <= 0)
            {
                throw new BadRequestException($"Invalid id value: {id}");
            }

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} was not found!");
            }

            if (product.Password != updateProductModel.Password)
            {
                throw new BadRequestException("Password is inccorect!");
            }

            if (!string.IsNullOrWhiteSpace(updateProductModel.Name))
            {
                product.Name = updateProductModel.Name;
            }

            if (!string.IsNullOrWhiteSpace(updateProductModel.Nickname))
            {
                product.Nickname = updateProductModel.Nickname;
            }

            if (!string.IsNullOrWhiteSpace(updateProductModel.Contact))
            {
                product.Contact = updateProductModel.Contact;
            }

            if (updateProductModel.Price != null)
            {
                product.Price = updateProductModel.Price;
            }

            if (updateProductModel.Count != null)
            {
                product.Count = updateProductModel.Count;
            }

            _productRepository.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
