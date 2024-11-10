using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IRepository<T> where T : class 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate=null);
    }
}
