using Microsoft.EntityFrameworkCore;
using TaskList.Domain.Entities;
using TaskList.Domain.Repositories;

namespace TaskList.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskListDbContext _dbContext;
    private readonly IPasswordHasher _hasher;

    public UserRepository(TaskListDbContext dbContext, IPasswordHasher hasher)
    {
        _dbContext = dbContext;
        _hasher = hasher;
    }

    public async Task<User?> FindAsync(string username)
    {
        var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        return result;
    }

    public async Task<User?> FindAsync(string username, string password)
    {
        var user = await FindAsync(username);
        if (user == null) return null;

        var isValid = _hasher.Verify(user.PasswordHash, password);
        if (!isValid) return null;

        return user;
    }
}