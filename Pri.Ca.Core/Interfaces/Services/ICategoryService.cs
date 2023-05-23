using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<ItemResultModel<Category>> GetAllAsync();
        Task<ItemResultModel<Category>> GetAsync(int id);
        ItemResultModel<Category> GetAll();
        Task<ItemResultModel<Category>> AddAsync(string name);
        Task<ItemResultModel<Category>> DeleteAsync(int id);
        Task<ItemResultModel<Category> > UpdateAsync(int id,string name);
    }
}
