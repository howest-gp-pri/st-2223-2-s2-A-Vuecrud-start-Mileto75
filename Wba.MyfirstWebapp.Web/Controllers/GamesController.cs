using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pri.Ca.Core.Interfaces.Services;
using Wba.MyfirstWebapp.Web.ViewModels;

namespace Wba.MyfirstWebapp.Web.Controllers
{

    public class GamesController : Controller
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [Authorize(Policy = "User")]
        public async Task<IActionResult> Index()
        {
            GamesIndexViewModel gamesIndexViewModel = new();

            //access service layer
            var result = await _gameService.GetAllAsync();
            if(!result.IsSuccess) 
            {
                //pass error to view
                gamesIndexViewModel.IsSuccess = false;
                gamesIndexViewModel.Errors = result.ValidationErrors.Select(v => v.ErrorMessage);
                return View(gamesIndexViewModel);
            }
            gamesIndexViewModel.Games = result.Items.Select(r => new BaseViewModel 
            {
                Id =  r.Id,
                Name = r.Name,
            });
            gamesIndexViewModel.IsSuccess = true;
            return View(gamesIndexViewModel);
        }
        
        public async Task<IActionResult> Test()
        {
            var result = await _gameService.AddAsync("Fifa2003",
                new List<int> { 1, 3 },null);
            var games = await _gameService.GetAllAsync();
            result = await _gameService.UpdateAsync(1, "Fiffa20", new List<int> { 1 });
            games = await _gameService.GetAllAsync();
            result = await _gameService.DeleteAsync(1);
            return Content("Test");
        }
        [Authorize(Policy = "18+")]
        public IActionResult AdultsOnly()
        {
            return Content("18+");
        }
    }
}
