using Infrastructure.Repositories.UserRepository;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    void Commit();
    Task CommitAsync(CancellationToken cancellationToken);
}