using Models.Core;
using System.Linq.Expressions;

namespace DAL.Repository.Core;
public interface IBaseRepo<T> where T : BaseModel
{
    IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    T? Get(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);


    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task<T?> GetByIdAsync(int id, string? includeProperties = null);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> IsExistAsync(Expression<Func<T, bool>>? filter = null);

}
