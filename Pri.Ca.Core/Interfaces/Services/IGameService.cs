using Microsoft.AspNetCore.Http;
using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Interfaces.Services
{
    public interface IGameService
    {
        //Get methods
        Task<ItemResultModel<Game>> GetAllAsync();
        Task<ItemResultModel<Game>> GetByIdAsync(int id);
        Task<ItemResultModel<Game>> AddAsync(string title,IEnumerable<int> categoryIds, IFormFile image);
        Task<ItemResultModel<Game>> DeleteAsync(int id);
        Task<ItemResultModel<Game>> UpdateAsync(int id, string title,IEnumerable<int> categoryIds);
        ItemResultModel<Game> SearchByTitleAsync(string title);
    }
}
