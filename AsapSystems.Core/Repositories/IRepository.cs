using System.Linq.Expressions;

namespace AsapSystems.Core.Repositories
{
    public interface IRepository<T>
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        Task ReloadAsync(T entity);
    }
}