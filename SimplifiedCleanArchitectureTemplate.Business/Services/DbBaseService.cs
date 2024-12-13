namespace SimplifiedCleanArchitectureTemplate.Business.Services;

public class DbBaseService<T> : IDbBaseService<T> where T : Entity
{
    protected readonly IDbBase<T> _database;

    public DbBaseService(IDbBase<T> database)
    {
        _database = database;
    }

    public virtual async Task<bool> CreateAsync(T entity)
        => await _database.CreateAsync(entity);

    public virtual async Task<IQueryable<KeyValuePair<string, T>>> GetAsync(Expression<Func<KeyValuePair<string, T>, bool>> predicate)
        => await _database.GetAsync(predicate);

    public virtual async Task<bool> UpdateAsync(T entity)
        => await _database.UpdateAsync(entity);

    public virtual async Task<bool> DeleteAsync(string id)
        => await _database.DeleteAsync(id);
}