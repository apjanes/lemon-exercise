namespace TaskList.Domain.Entities;

public class User
{
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public Guid Id { get; set; }

    // DEBUG: add info about security stamp
    public required string Username { get; set; }

    public required string PasswordHash { get; set; }
}