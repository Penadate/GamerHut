global using BLL.Models.Game;
using BLL.Exceptions;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using System.Xml.Linq;

namespace GamerHutWeb.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
        public async Task<IActionResult> Index(string operation)
        {
            ViewData["operation"] = operation;
            var games = await _gameService.GetGamesAsync();
            return View(games.OrderBy(game => game.Name));
        }

        public async Task<IActionResult> Create(string operation)
        {
            ViewData["operation"] = operation;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameModel createGameModel, string operation)
        {
            ViewData["operation"] = operation;
            try
            {
                if (ModelState.IsValid)
                {
                    var game = await _gameService.CreateGameAsync(createGameModel);
                    TempData["success"] = "Game added successfully";
                    return RedirectToAction("Index", new {operation});
                }
                return View(createGameModel);
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError("CustomError", $"Game with name {createGameModel.Name} already Exist");
                return View(createGameModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        public async Task<IActionResult> Edit(int id, string operation)
        {
            ViewData["operation"] = operation;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateGameModel updateGameModel, string operation)
        {
            ViewData["operation"] = operation;
            if (updateGameModel.Keyword != "Ntvxbr2003")
            {
                ModelState.AddModelError("CustomError", "Keyword is incorrect");
                return View(updateGameModel);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    await _gameService.UpdateAsync(id, updateGameModel);
                    TempData["success"] = "Game edited successfully";
                    return RedirectToAction("Index", new {operation});
                }
                return View(updateGameModel);

            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(updateGameModel);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
                return View(updateGameModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        public async Task<IActionResult> Delete(int id, string operation)
        {
            ViewData["operation"] = operation;
            ViewData["Id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, PasswordModel passwordModel, string operation)
        {
            ViewData["Id"] = id;
            ViewData["operation"] = operation;
            if (passwordModel.Password != "Ntvxbr2003")
            {
                ModelState.AddModelError("CustomError", "Keyword is incorrect");
                return View();
            }
            try
            {
                await _gameService.DeleteAsync(id);
                TempData["success"] = "Game deleted successfully";
                return RedirectToAction("Index", new { operation });

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
    }
}
