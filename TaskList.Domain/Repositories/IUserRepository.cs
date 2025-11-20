using TaskList.Domain.Entities;

namespace TaskList.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> FindAsync(string username);

    Task<User?> FindAsync(string username, string password);
}