using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pri.Ca.Api.DTOs;
using Pri.Ca.Api.DTOs.Games;
using Pri.Ca.Core.Entities;

namespace Pri.Ca.Api.Extensions
{
    public static class Mappers
    {
        //extension
        public static GameResponseDto MaptoDto(this Game game,IHttpContextAccessor httpContextAccessor)
        {
            var scheme = httpContextAccessor.HttpContext.Request.Scheme;
            var host = httpContextAccessor.HttpContext.Request.Host;
            var path = $"{scheme}://{host}/images/{typeof(Game).Name}";
            return new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Categories = game.Categories.Select
                (c => new BaseResponseDto 
                {
                    Id = c.Id,
                    Name = c.Name,
                }),
                Image = $"{path}/{game.Image ?? "placeholder.jpg"}",

            };
        }
        //map from IEnumerable<Game> => GamesresponseDto
        public static GamesResponseDto MapToDto(this IEnumerable<Game> games,IHttpContextAccessor httpContextAccessor)
        {
            return new GamesResponseDto
            {
                Games = games.Select(g => g.MaptoDto(httpContextAccessor))
            };
        }
    }
}

