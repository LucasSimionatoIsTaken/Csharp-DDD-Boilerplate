using Core;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.UserRepository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(Context context) : base(context)
    {
    }
}