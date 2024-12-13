namespace SimplifiedCleanArchitectureTemplate.Persistence.Databases;

public class InMemDatabase<T> : IDbBase<T> where T : Entity
{
    private readonly Dictionary<string, T> _database;

    public InMemDatabase()
    {
        _database = [];
    }

    public Task<bool> CreateAsync(T entity)
    {
        if (_database.ContainsKey(entity.Id))
        {
            return Task.Run(() => false);
        }

        _database.Add(entity.Id, entity);
        return Task.Run(() => true);
    }

    public Task<IQueryable<KeyValuePair<string, T>>> GetAsync(Expression<Func<KeyValuePair<string, T>, bool>> predicate)
    {
        return Task.Run(() => _database.AsQueryable().Where(predicate));
    }

    public Task<bool> UpdateAsync(T entity)
    {
        if (!_database.ContainsKey(entity.Id))
        {
            return Task.Run(() => false);
        }

        _database[entity.Id] = entity;
        return Task.Run(() => true);
    }

    public Task<bool> DeleteAsync(string id)
    {
        return Task.Run(() => _database.Remove(id));
    }
}