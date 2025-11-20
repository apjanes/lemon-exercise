namespace TaskList.Domain.Entities;

public class User
{
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public Guid Id { get; set; }

    // DEBUG: add info about security stamp
    public required string Username { get; set; }

    public required string PasswordHash { get; set; }
}