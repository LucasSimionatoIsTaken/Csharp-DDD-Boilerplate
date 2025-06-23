using Infrastructure.Repositories.UserRepository;

namespace Infrastructure.SeedWork.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    
    void Commit();
    Task CommitAsync(CancellationToken cancellationToken);
}