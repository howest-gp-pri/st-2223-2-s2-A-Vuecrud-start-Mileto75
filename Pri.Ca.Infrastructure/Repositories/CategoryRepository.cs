using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Interfaces.Repositories;
using Pri.Ca.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbcontext applicationDbcontext, ILogger<GameRepository> logger) : base(applicationDbcontext, logger)
        {
        }

        public override IQueryable<Category> GetAll()
        {
            return _applicationDbcontext
                .Categories
                .Include(g => g.Games)
                .AsQueryable();
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _applicationDbcontext
                .Categories
                .Include(g => g.Games)
                .ToListAsync();
        }

        public override async Task<Category> GetByIdAsync(int id)
        {
            return await _applicationDbcontext
                .Categories
                .Include(g => g.Games)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
