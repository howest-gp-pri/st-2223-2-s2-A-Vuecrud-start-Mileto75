using Microsoft.AspNetCore.Http;
using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Interfaces.Repositories;
using Pri.Ca.Core.Interfaces.Services;
using Pri.Ca.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageService _imageService;

        public GameService(IGameRepository gameRepository, ICategoryRepository categoryRepository, IImageService imageService)
        {
            _gameRepository = gameRepository;
            _categoryRepository = categoryRepository;
            _imageService = imageService;
        }

        public async Task<ItemResultModel<Game>> AddAsync(string title, IEnumerable<int> categoryIds, IFormFile image)
        {
            //check if title exists
            var games = await _gameRepository.GetAllAsync();
            if(games.Any(g => g.Name.Contains(title)))
            {
                //game exists error
                return new ItemResultModel<Game>
                {
                    ValidationErrors = new List<ValidationResult>
                    { new ValidationResult("Title exists!") }
                };
            }
            //check if every categoryId exists in db
            if(!_categoryRepository.GetAll().Any(c => categoryIds.Contains(c.Id)))
            {
                //game exists error
                return new ItemResultModel<Game>
                {
                    ValidationErrors = new List<ValidationResult>
                    { new ValidationResult("Category id not found!") }
                };
            }
            //handle image upload
            var fileName = "";
            if(image != null)
            {
                var result = await _imageService.StoreImageAsync<Game>(image);
                //check for errors
                if(!result.IsSuccess)
                {
                    return new ItemResultModel<Game>
                    {
                        ValidationErrors = new List<ValidationResult>
                        {new ValidationResult("Something went wrong...")}
                    };
                }
                fileName = result.Image;
            }
            var game = new Game 
            {
                Name = title,
                Categories = _categoryRepository.GetAll()
                .Where(c => categoryIds.Contains(c.Id)).ToList(),
                Image = fileName
            };
            //add to database
            if(!await _gameRepository.AddAsync(game))
            {
                return new ItemResultModel<Game>
                {
                    ValidationErrors = new List<ValidationResult>
                    { new ValidationResult("Something went wrong...") }
                };
            }
            //game saved
            return new ItemResultModel<Game>
            {
                IsSuccess = true,
            };
        }

        public async Task<ItemResultModel<Game>> DeleteAsync(int id)
        {
            //check if game exists
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return new ItemResultModel<Game>
                {
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Game not found!")
                    }
                };
            }
            //check for image
            if(game.Image != null)
            {
                //delete the image
                var result = _imageService.DeleteImage<Game>(game.Image);
                if(!result.IsSuccess)
                {
                    return new ItemResultModel<Game>
                    {
                        ValidationErrors = new List<ValidationResult> 
                        {
                           new ValidationResult("Something went wrong...")
                        }
                    };
                }
            }
            //safely delete
            if(!await _gameRepository.DeleteAsync(game.Id))
            {
                //check if game exists
                return new ItemResultModel<Game>
                {
                        ValidationErrors = new List<ValidationResult>
                {
                            new ValidationResult("Something went wrong...")}
                };
            }
            //delete success
            return new ItemResultModel<Game> { IsSuccess = true };
        }
        

        public async Task<ItemResultModel<Game>> GetAllAsync()
        {
            var gamesItemResultModel = new ItemResultModel<Game>();
            var games = await _gameRepository.GetAllAsync();
            //validation
            if (games.Count() == 0)
            {
                gamesItemResultModel.IsSuccess = false;
                gamesItemResultModel.ValidationErrors
                    = new List<ValidationResult>
                    {
                        new ValidationResult("No Games in database!")
                    };
                return gamesItemResultModel;
            }
            gamesItemResultModel.IsSuccess = true;
            gamesItemResultModel.Items = games;
            return gamesItemResultModel;
        }

        

        public async Task<ItemResultModel<Game>> GetByIdAsync(int id)
        {
            var gamesItemResultModel = new ItemResultModel<Game>();
            var game = await _gameRepository.GetByIdAsync(id);
            //validation
            if(game == null) 
            {
                gamesItemResultModel.IsSuccess = false;
                gamesItemResultModel.ValidationErrors = new List<ValidationResult>
                {
                    new ValidationResult("Game not found!")
                };
                return gamesItemResultModel;
            }
            if(game.Image != null)
            {
                var result = _imageService.GetImagePath<Game>(game.Image);
                game.Image = result.Image;
            }
            gamesItemResultModel.IsSuccess = true;
            gamesItemResultModel.Items = new List<Game> { game };
            return gamesItemResultModel;
        }

        public ItemResultModel<Game> SearchByTitleAsync(string title)
        {
            var results = _gameRepository.GetAll()
                .Where(g => g.Name.ToLower().Contains(title.ToLower()));
            if (results.Count() == 0)
            {
                return new ItemResultModel<Game>
                {
                    IsSuccess = false,
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("No games found!")
                    }
                };
            }
            return new ItemResultModel<Game>
            {
                Items = results,
                IsSuccess = true
            };
        }

        public async Task<ItemResultModel<Game>> UpdateAsync(int id, string title, IEnumerable<int> categoryIds)
        {
            //check if game exists
            var game = await _gameRepository.GetByIdAsync(id);
            if(game == null) 
            {
                return new ItemResultModel<Game>
                {
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("Game not found!")
                    }
                };
            }
            //check if every categoryId exists in db
            if (!_categoryRepository.GetAll().Any(c => categoryIds.Contains(c.Id)))
            {
                //game exists error
                return new ItemResultModel<Game>
                {
                    ValidationErrors = new List<ValidationResult>
                    { new ValidationResult("Category id not found!") }
                };
            }
            //update the game
            game.Name = title;
            game.Categories = _categoryRepository.GetAll()
                .Where(c => categoryIds.Contains(c.Id)).ToList();
            //save to database
            if(!await _gameRepository.UpdateAsync(game))
            {
                return new ItemResultModel<Game>
                {
                    ValidationErrors = new List<ValidationResult>
                    {
                        new ValidationResult("something went wrong...")
                    }
                };
            }
            return new ItemResultModel<Game> { IsSuccess = true };
        }
    }
}