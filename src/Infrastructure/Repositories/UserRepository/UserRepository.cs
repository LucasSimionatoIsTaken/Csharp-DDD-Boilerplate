using Core;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UserRepository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await AppDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}