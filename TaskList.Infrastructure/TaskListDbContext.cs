using Microsoft.EntityFrameworkCore;
using TaskList.Domain.Entities;

namespace TaskList.Infrastructure;

public class TaskListDbContext : DbContext
{
    public TaskListDbContext(DbContextOptions<TaskListDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<WorkItem> WorkItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new WorkItemConfiguration());
    }
}