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
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbcontext _applicationDbcontext;
        protected ILogger<GameRepository> _logger;
        protected DbSet<T> _table;

        public BaseRepository(ApplicationDbcontext applicationDbcontext, ILogger<GameRepository> logger)
        {
            _applicationDbcontext = applicationDbcontext;
            _logger = logger;
            _table = _applicationDbcontext.Set<T>();
        }

        public async Task<bool> AddAsync(T toAdd)
        {
            await _table.AddAsync(toAdd);
            return await Save();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var t = await GetByIdAsync(id);
            _table.Remove(t);
            return await Save();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _table.AsQueryable();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _table
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> UpdateAsync(T toUpdate)
        {
            _table.Update(toUpdate);
            return await Save();
        }
        private async Task<bool> Save()
        {
            try
            {
                await _applicationDbcontext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbUpdateException)
            {
                //log the error
                _logger.LogError(dbUpdateException.Message);
                return false;
            }
        }
    }
}
