using Core.SeedWork;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : GenericModel
{
    protected GenericRepository(Context context)
    {
        _context = context;
        _entity = _context.Set<T>();
    }

    protected readonly Context _context;
    private DbSet<T> _entity;
    
    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _entity.AddAsync(entity, cancellationToken);
    }

    public async Task<List<T>> ListAllAsync(CancellationToken cancellationToken)
    {
        return await _entity.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Guid id, T entity, CancellationToken cancellationToken)
    {
        await Task.FromResult(_entity.Update(entity));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        T entity = await _entity.FindAsync(id);

        entity.SetDeletedAt();
    }
}