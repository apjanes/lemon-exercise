using Microsoft.EntityFrameworkCore;
using TaskList.Backend.Data.Entities;

namespace TaskList.Backend.Data;

public class TaskListDbContext : DbContext
{
    public TaskListDbContext(DbContextOptions<TaskListDbContext> options) : base(options)
    {
    }

    public DbSet<WorkItem> WorkItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new WorkItem.Configuration());
    }
}