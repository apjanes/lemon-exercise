using Microsoft.EntityFrameworkCore;
using TaskList.Backend.Data.Entities;

namespace TaskList.Backend.Data.Repositories;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly TaskListDbContext _dbContext;

    public WorkItemRepository(TaskListDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WorkItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext
            .WorkItems
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task SaveAsync(WorkItem workItem, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.FindAsync<WorkItem>(workItem.Id);

        if (existing != null)
        {
            existing.Update(workItem);
        }
        else
        {
            _dbContext.WorkItems.Add(workItem);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}