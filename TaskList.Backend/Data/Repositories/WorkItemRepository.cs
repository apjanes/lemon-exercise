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
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<WorkItem?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FindAsync<WorkItem>(cancellationToken);
    }

    public async Task<WorkItem> SaveAsync(WorkItem workItem, CancellationToken cancellationToken = default)
    {
        // Attempt to get an existing entity with the same ID.
        var existing = await _dbContext.FindAsync<WorkItem>(workItem.Id, cancellationToken);

        if (existing == null)
        {
            // If there is no existing entity, add it and save.
            _dbContext.WorkItems.Add(workItem);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return workItem;
            }
            catch (DbUpdateException exception) when (exception.IsUniqueKeyViolation())
            {
                // In the event that another request or process created the entity between checking and saving,
                // detach the entity and load the existing one.
                _dbContext.Entry(workItem).State = EntityState.Detached;
                existing = await _dbContext.FindAsync<WorkItem>(workItem.Id, cancellationToken);
            }
        }

        // Update the entity and save.
        existing!.UpdateFrom(workItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return workItem;
    }
}