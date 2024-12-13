namespace SimplifiedCleanArchitectureTemplate.Business.Services;

public interface IDbBaseService<T> where T : Entity
{
    Task<bool> CreateAsync(T entity);
    Task<IQueryable<KeyValuePair<string, T>>> GetAsync(Expression<Func<KeyValuePair<string, T>, bool>> predicate);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
}