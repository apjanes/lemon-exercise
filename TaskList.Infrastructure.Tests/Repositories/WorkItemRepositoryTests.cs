using Microsoft.EntityFrameworkCore;
using TaskList.Domain.Entities;
using TaskList.Infrastructure.Repositories;

namespace TaskList.Infrastructure.Tests.Repositories;

public class WorkItemRepositoryTests
{
    private readonly TaskListDbContext _dbContext;

    public WorkItemRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TaskListDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TaskListDbContext(options);
    }

    public async Task SetComplete_WhenCalledWithValidIdAndTrue_ThenCompletedAtIsSet()
    {
        // Arrange
        var id = CombGuid.NewGuid();
        var existing = new WorkItem(id, "title", "description");

        _dbContext.Add(existing);
        await _dbContext.SaveChangesAsync();

        // Act
        var sut = CreateSut();
        await sut.SetComplete(id, true);
    }

    private WorkItemRepository CreateSut()
    {
        return new WorkItemRepository(_dbContext);
    }
}