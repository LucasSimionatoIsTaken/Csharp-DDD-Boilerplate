using Infrastructure.Contexts;
using Infrastructure.Repositories.UserRepository;

namespace Infrastructure.SeedWork.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _appDbContext;
    private bool _isDisposed = false;
    public IUserRepository UserRepository { get; }
    
    public UnitOfWork(AppDbContext appDbContext, IUserRepository UserRepository)
    {
        _appDbContext = appDbContext;
        this.UserRepository = UserRepository;
    }

    public void Commit()
    {
        _appDbContext.SaveChanges();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool idDisposing)
    {
        if (this._isDisposed) return;
        
        if (idDisposing)
            _appDbContext.Dispose();
            
        this._isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}