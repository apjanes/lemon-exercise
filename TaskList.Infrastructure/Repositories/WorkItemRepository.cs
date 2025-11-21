using Microsoft.EntityFrameworkCore;
using TaskList.Domain.Entities;
using TaskList.Domain.Repositories;
using TaskList.Infrastructure.Extensions;

namespace TaskList.Infrastructure.Repositories;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly TaskListDbContext _dbContext;

    public WorkItemRepository(TaskListDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var toDelete = await FindAsync(id, cancellationToken);

        if (toDelete != null)
        {
            _dbContext.Remove(toDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }
    public async Task<WorkItem> CreateAsync(WorkItem workItem, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.WorkItems.Add(workItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.IsUniqueKeyViolation())
        {
            // In the event that another request or process created the entity between checking and saving,
            // detach the entity and load the existing one.
            _dbContext.Entry(workItem).State = EntityState.Detached;
            var result = await UpdateAsync(workItem.Id, workItem, cancellationToken);

            // We know that an entity will be turned due to the exception.
            return result!;
        }

        return workItem;
    }

    public async Task<WorkItem?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FindAsync<WorkItem>(id, cancellationToken);
    }

    public async Task<IEnumerable<WorkItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext
            .WorkItems
            .OrderBy(x => x.CompletedAt)
            .ThenByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<WorkItem?> SetComplete(Guid id, bool isComplete, CancellationToken cancellationToken = default)
    {
        var existing = await FindAsync(id, cancellationToken);
        if (existing == null) return null;

        if (existing.IsComplete != isComplete)
        {
            existing.CompletedAt = isComplete ? DateTime.UtcNow : null;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<WorkItem?> UpdateAsync(Guid id, WorkItem workItem, CancellationToken cancellationToken)
    {
        var existing = await FindAsync(id, cancellationToken);

        if (existing == null) return null;

        existing.Update(workItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return existing;
    }
}