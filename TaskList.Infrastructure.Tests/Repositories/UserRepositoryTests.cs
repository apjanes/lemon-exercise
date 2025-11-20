using Microsoft.EntityFrameworkCore;
using TaskList.Domain.Entities;
using TaskList.Infrastructure.Repositories;
using Xunit;

namespace TaskList.Infrastructure.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly TaskListDbContext _dbContext;
    private readonly PasswordHasher _hasher;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TaskListDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TaskListDbContext(options);
        _hasher = new PasswordHasher();
    }

    [Fact]
    public async Task Find_WhenCalledWithNonExistingUser_ReturnsNull()
    {
        // Arrange
        const string username = "non-existent";

        // Act
        var sut = CreateSut();
        var result = await sut.FindAsync(username);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task FindAsync_WhenCalledWithExistingUser_ReturnsUser()
    {
        // Arrange
        const string username = "valid";

        await PopulateUserAsync(username);

        // Act
        var sut = CreateSut();
        var result = await sut.FindAsync(username);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
    }

    [Fact]
    public async Task FindAsync_WhenCalledWithExistingUserAndInvalidPassword_ReturnsNull()
    {
        // Arrange
        const string username = "valid";
        const string password = "invalid";

        await PopulateUserAsync(username);


        // Act
        var sut = CreateSut();
        var result = await sut.FindAsync(username, password);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindAsync_WhenCalledWithExistingUserAndPassword_ReturnsUser()
    {
        // Arrange
        const string username = "valid";
        const string password = "correct";

        await PopulateUserAsync(username, password);

        // Act
        var sut = CreateSut();
        var result = await sut.FindAsync(username, password);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task FindAsync_WhenCalledWithNonExistingUserAndPassword_ReturnsNull()
    {
        // Arrange
        const string username = "non-existent";
        const string password = "irrelevant";


        // Act
        var sut = CreateSut();
        var result = await sut.FindAsync(username, password);

        // Assert
        Assert.Null(result);
    }

    private UserRepository CreateSut()
    {
        return new UserRepository(_dbContext, new PasswordHasher());
    }

    private async Task PopulateUserAsync(string username, string? password = null)
    {
        password = password ?? string.Empty;

        var hashed = _hasher.Hash(password);
        _dbContext.Users.Add(new User
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            Username = username,
            PasswordHash = hashed,
        });

        await _dbContext.SaveChangesAsync();
    }
}