using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly MyContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(MyContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();

        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await GetAsync(predicate);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Entity not found for the provided predicate.");
            }
            //delete
            //exist async
        }
    }
}
