using Microsoft.EntityFrameworkCore;
using TaskList.Backend.Data.Entities;
using TaskList.Backend.Extensions;

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
            .OrderBy(x => x.IsComplete)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<WorkItem?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FindAsync<WorkItem>(cancellationToken);
    }

    public async Task<WorkItem> SaveAsync(WorkItem workItem, bool isNew, CancellationToken cancellationToken = default)
    {
        if (isNew)
        {
            try
            {
                return await CreateAsync(workItem, cancellationToken);
            }
            catch (DbUpdateException exception) when (exception.IsUniqueKeyViolation())
            {
                // In the event that another request or process created the entity between checking and saving,
                // detach the entity and load the existing one.
                _dbContext.Entry(workItem).State = EntityState.Detached;
                return await UpdateAsync(workItem, cancellationToken);
            }
        }

        return await UpdateAsync(workItem, cancellationToken);
    }

    private async Task<WorkItem> CreateAsync(WorkItem workItem, CancellationToken cancellationToken)
    {
        _dbContext.WorkItems.Add(workItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return workItem;
    }

    private async Task<WorkItem> UpdateAsync(WorkItem workItem, CancellationToken cancellationToken)
    {
        // DEBUG: Handle error
        var existing = await _dbContext.FindAsync<WorkItem>(workItem.Id, cancellationToken);
        existing!.UpdateFrom(workItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return existing;
    }
}