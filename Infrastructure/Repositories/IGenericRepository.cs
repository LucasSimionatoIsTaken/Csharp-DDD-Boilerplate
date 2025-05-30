namespace Infrastructure.Repositories;

public interface IGenericRepository<T>
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    
    Task<List<T>> ListAllAsync(CancellationToken cancellationToken); // pensar no mapster
    
    Task UpdateAsync(Guid id, T entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    // Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    //list by
}