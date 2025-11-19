using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskList.Backend.Data.Entities;

public class User
{
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public Guid Id { get; set; }

    // DEBUG: add info about security stamp
    public required string Username { get; set; }

    public required string PasswordHash { get; set; }

    public class Configuration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .Property(x => x.Username)
                .HasMaxLength(256);

            builder
                .Property(x => x.PasswordHash)
                .HasMaxLength(512);
        }
    }
}