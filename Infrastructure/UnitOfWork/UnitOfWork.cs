using Infrastructure.Contexts;
using Infrastructure.Repositories.UserRepository;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly Context _context;
    private bool _isDisposed = false;
    public IUserRepository UserRepository { get; }
    
    public UnitOfWork(Context context, IUserRepository UserRepository)
    {
        _context = context;
        this.UserRepository = UserRepository;
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool idDisposing)
    {
        if (this._isDisposed) return;
        
        if (idDisposing)
            _context.Dispose();
            
        this._isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}