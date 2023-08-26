global using BLL.Models.Category;
using BLL.Exceptions;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;

namespace GamerHutWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int gameId, string operation)
        {
            try
            {
                ViewData["operation"] = operation;
                ViewData["GameId"] = gameId;
                var categories = await _categoryService.GetCategoriesByGameIdAsync(gameId);
                return View(categories.OrderBy(category => category.Name));
            }
            catch (BadRequestException ex)
            {
                TempData["success"] = ex.Message;
                ModelState.AddModelError("CustomError", ex.Message);
                return View();
            }
            catch (NotFoundException ex)
            {
                TempData["success"] = ex.Message;
                ModelState.AddModelError("CustomError", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                TempData["success"] = ex.Message;
                return View();
            }
        }
        public async Task<IActionResult> Create(int gameId, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryModel createCategoryModel, string operation)
        {
            try
            {
                ViewData["operation"] = operation;
                ViewData["GameId"] = createCategoryModel.GameId;
                if (ModelState.IsValid)
                {
                    var game = await _categoryService.CreateCategoryAsync(createCategoryModel);
                    TempData["success"] = "Category added successfully";
                    return RedirectToAction("Index", new { gameId = createCategoryModel.GameId, operation});
                }
                return View(createCategoryModel);

            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError("CustomError", $"Category with name {createCategoryModel.Name} already Exist");
                return View(createCategoryModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        public async Task<IActionResult> Edit(int id, int gameId, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["Id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int gameId, int id, UpdateCategoryModel updateCategoryModel, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["Id"] = id;
            if (updateCategoryModel.Keyword != "Ntvxbr2003")
            {
                ModelState.AddModelError("CustomError", "Keyword is incorrect");
                return View(updateCategoryModel);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryService.UpdateAsync(id, updateCategoryModel);
                    TempData["success"] = "Category edited successfully";
                    return RedirectToAction("Index", new { gameId = ViewData["GameId"], operation });
                }
                return View(updateCategoryModel);

            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(updateCategoryModel);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(updateCategoryModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        public async Task<IActionResult> Delete(int id, int gameId, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["Id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int gameId, PasswordModel passwordModel, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["Id"] = id;

            if (passwordModel.Password != "Ntvxbr2003")
            {
                ModelState.AddModelError("CustomError", "Keyword is incorrect");
                return View();
            }
            try
            {
                await _categoryService.DeleteAsync(id);
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index", new { gameId = ViewData["GameId"], operation });

            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View();
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
