using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pri.Ca.Api.DTOs;
using Pri.Ca.Api.DTOs.Games;
using Pri.Ca.Api.Extensions;
using Pri.Ca.Core.Interfaces.Services;
using Pri.Ca.Core.Services;
using System.Security.Claims;

namespace Pri.Ca.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GamesController(IGameService gameService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _gameService = gameService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            //get the userId from authenticated user
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //get the games  
            var result = await _gameService.GetAllAsync();
            //check for errors
            if(!result.IsSuccess)
            {
                return NotFound(result.ValidationErrors);
            }
            return Ok(result.Items.MapToDto(_httpContextAccessor));
            //return Ok(_mapper.Map<GamesResponseDto>(result.Items));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _gameService
                .GetByIdAsync(id);
            if(!result.IsSuccess)
            {
                return NotFound(result.ValidationErrors);
            }
            return Ok(result.Items.First().MaptoDto(_httpContextAccessor));
            //return Ok(_mapper.Map<GameResponseDto>(result.Items.First()));
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Add([FromForm]GameRequestDto gameRequestDto)
        {
            if(gameRequestDto.Categories.Count() == 0)
            {
                ModelState.AddModelError("Categories",
                    "Categories missing!");
                return BadRequest(ModelState.Values);
            }
            //store to database
            var result = await _gameService.AddAsync
                (gameRequestDto.Name,gameRequestDto.Categories,gameRequestDto.Image);
            //check for errors
            if(!result.IsSuccess) 
            {
                return BadRequest(result.ValidationErrors);
            }
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(GameUpdateRequestDto gameUpdateRequestDto)
        {
            if (gameUpdateRequestDto.Categories.Count() == 0)
            {
                ModelState.AddModelError("Categories",
                    "Categories missing!");
                return BadRequest(ModelState.Values);
            }
            var result = await _gameService.UpdateAsync(gameUpdateRequestDto.Id,
                gameUpdateRequestDto.Name, gameUpdateRequestDto.Categories);
            //check result for errors
            if(!result.IsSuccess)
            {
                return BadRequest(result.ValidationErrors);
            }
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _gameService.DeleteAsync(id);
            if(!result.IsSuccess) 
            {
                return BadRequest(result.ValidationErrors);
            }
            return Ok();
        }
        [HttpGet("{title}")]
        public IActionResult SearchByTitle(string title) 
        {
            if(title == null)
            {
                title = "";
            }
            var results = _gameService.SearchByTitleAsync(title);
            if(!results.IsSuccess)
            {
                return BadRequest(results.ValidationErrors);
            }
            GamesResponseDto gamesResponseDto = new GamesResponseDto();
            gamesResponseDto.Games
                = results.Items.Select(i => new GameResponseDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Categories = i.Categories.Select(c => new BaseResponseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    })
                });
            return Ok(gamesResponseDto);
        }
    }
}
