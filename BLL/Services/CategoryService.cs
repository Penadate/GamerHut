using AutoMapper;
using BLL.Exceptions;
using BLL.Models.Category;
using BLL.Models.Game;
using BLL.Services.Interfaces;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CategoryService : ICategoryService            
    {
        private readonly AppDbContext _dbContext;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        public CategoryService(
            AppDbContext dbContext,
            ICategoryRepository categoryRepository,
            IGameRepository gameRepository,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _categoryRepository = categoryRepository;
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesByGameIdAsync(int gameId)
        {
            if (gameId <= 0)
            {
                throw new BadRequestException($"Invalid id value: {gameId}");
            }

            var categories = await _categoryRepository.GetCategoriesByGameIdAsync(gameId);
            if(categories == null)
            {
                throw new NotFoundException($"Categories with game id  {gameId} was not found");
            }
            return _mapper.Map<IEnumerable<CategoryModel>>(categories);
        }

        public async Task<CategoryModel> CreateCategoryAsync(CreateCategoryModel createCategoryModel)
        {
            var check = await _categoryRepository.GetCategoryByNameAsync(createCategoryModel.Name);

            if (check != null)
            {
                throw new BadRequestException($"Category with name \"{check.Name}\" already exists!");
            }

            if (createCategoryModel.GameId <= 0)
            {
                throw new BadRequestException($"Invalid id value: {createCategoryModel.GameId}");
            }

            var category = new Category();

            var game = await _gameRepository.GetGameByIdAsync(createCategoryModel.GameId)
                ?? throw new NotFoundException($"Game with id \"{createCategoryModel.GameId}\" was not found!");

            category.Game = game;
            category.GameId = game.Id;
            category.Name = createCategoryModel.Name;

            await _categoryRepository.CreateAsync(category);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException($"Invalid id value: {id}");
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(id)
               ?? throw new NotFoundException($"Category with id {id} was not found!");

            _categoryRepository.Delete(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateCategoryModel updateCategoryModel)
        {
            if (id <= 0)
            {
                throw new BadRequestException($"Invalid id value: {id}");
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(id)
               ?? throw new NotFoundException($"Category with id {id} was not found!");

            var check = await _categoryRepository.GetCategoryByNameAsync(updateCategoryModel.Name);

            if (check != null)
            {
                throw new BadRequestException($"Category with name {updateCategoryModel.Name} alreasy Exist!");
            }

            if (!string.IsNullOrWhiteSpace(updateCategoryModel.Name))
            {
                category.Name = updateCategoryModel.Name;
            }

            _categoryRepository.Update(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
