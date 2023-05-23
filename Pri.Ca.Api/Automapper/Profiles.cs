using AutoMapper;
using Pri.Ca.Api.DTOs;
using Pri.Ca.Api.DTOs.Games;
using Pri.Ca.Core.Entities;

namespace Pri.Ca.Api.Automapper
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Game, GameResponseDto>()
                .ForMember(d => d.Categories, s => s.MapFrom(s => s.Categories
                .Select(c => new BaseResponseDto 
                {
                    Id = c.Id,
                    Name = c.Name,
                })));
            CreateMap<IEnumerable<Game>, GamesResponseDto>()
                .ForMember(d => d.Games, s => s.MapFrom(s => s
                .Select(g => new GameResponseDto 
                {
                    Id = g.Id,
                    Name = g.Name,
                    Categories = g.Categories.Select(c => new BaseResponseDto 
                    {
                        Id = c.Id,
                        Name = c.Name,
                    })
                })));
        }
    }
}
