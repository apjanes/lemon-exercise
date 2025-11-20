using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskList.Infrastructure;

public class TaskListDbContextFactory : IDesignTimeDbContextFactory<TaskListDbContext>
{
    public TaskListDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TaskListDbContext>()
            .UseSqlite("Data Source=..\\TaskList.WebApi\\tasklist.sqlite")
            .Options;

        return new TaskListDbContext(options);
    }
}