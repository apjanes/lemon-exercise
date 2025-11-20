using TaskList.Domain.Entities;

namespace TaskList.Domain.Repositories;

public interface IWorkItemRepository
{
    public Task<WorkItem> CreateAsync(WorkItem workItem, CancellationToken cancellationToken = default);

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<WorkItem>> ListAsync(CancellationToken cancellationToken = default);

    public Task<WorkItem?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<WorkItem?> SetComplete(Guid id, bool isComplete, CancellationToken cancellationToken = default);

    public Task<WorkItem?> UpdateAsync(Guid id, WorkItem workItem, CancellationToken cancellationToken = default);
}