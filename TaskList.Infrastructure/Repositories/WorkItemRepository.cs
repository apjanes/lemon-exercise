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
            return await UpdateAsync(workItem.Id, workItem, cancellationToken);
        }

        return workItem;
    }

    public async Task<WorkItem?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FindAsync<WorkItem>(cancellationToken);
    }

    public async Task<IEnumerable<WorkItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext
            .WorkItems
            // DEBUG: fix and reinstate
            //.OrderBy(x => x.IsComplete)
            //.ThenByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<WorkItem> UpdateAsync(Guid id, WorkItem workItem, CancellationToken cancellationToken)
    {
        // DEBUG: Handle error
        var existing = await _dbContext.FindAsync<WorkItem>(id, cancellationToken);
        existing!.Update(workItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return existing;
    }
}