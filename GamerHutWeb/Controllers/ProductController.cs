global using BLL.Models.Product;
global using GamerHutWeb.Models;

using BLL.Exceptions;
using BLL.Services;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamerHutWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(int gameId, int categoryId, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["CategoryId"] = categoryId;
            var products = await _productService.GetProductsByCategoryIdAsync(categoryId);
            return View(products.OrderBy(product => product.Name));
        }

        public async Task<IActionResult> Detail(int gameId, int categoryId, int id, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["CategoryId"] = categoryId;
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return View(product);
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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        public async Task<IActionResult> Create(int gameId, int categoryId, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["CategoryId"] = categoryId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int gameId, int categoryId, CreateProductModel createProductModel, string operation)
        {
            try
            {
                ViewData["operation"] = operation;
                ViewData["CategoryId"] = createProductModel.CategoryId;
                ViewData["GameId"] = gameId;

                if (ModelState.IsValid)
                {
                    var game = await _productService.CreateProductAsync(createProductModel);
                    TempData["success"] = "Product added successfully";
                    return RedirectToAction("Index", new { gameId, categoryId, operation}); 
                }
                return View(createProductModel);

            }
            catch (NotFoundException ex)
            {
                return View(createProductModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        public async Task<IActionResult> Edit(int id, int gameId, int categoryId, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["CategoryId"] = categoryId;
            ViewData["Id"] = id;
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return View();
            }
            UpdateProductModel updateProductModel = new UpdateProductModel
            {
                Name = product.Name,
                Price = product.Price,
                Count = product.Count,
                Nickname = product.Nickname,
                Contact = product.Contact,
                Password = string.Empty!
            };
            return View(updateProductModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int gameId, int categoryId, int id, UpdateProductModel updateProductModel, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["CategoryId"] = categoryId;
            ViewData["Id"] = id;

            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.UpdateAsync(id, updateProductModel);
                    TempData["success"] = "Product edited successfully";
                    return RedirectToAction("Detail", new { gameId, categoryId, id, operation });
                }
                return View(updateProductModel);

            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(updateProductModel);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(updateProductModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        public async Task<IActionResult> Delete(int id, int gameId, int categoryId, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["CategoryId"] = categoryId;
            ViewData["Id"] = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int gameId, int categoryId, PasswordModel passwordModel, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["GameId"] = gameId;
            ViewData["CategoryId"] = categoryId;
            ViewData["Id"] = id;

            try
            {
                await _productService.DeleteAsync(id, passwordModel.Password);
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction("Index", new { gameId, categoryId, operation });

            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }

           return View();
        }
    }
}
