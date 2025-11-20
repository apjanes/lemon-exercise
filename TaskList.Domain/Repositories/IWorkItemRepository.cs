using TaskList.Domain.Entities;

namespace TaskList.Domain.Repositories;

public interface IWorkItemRepository
{
    public Task<IEnumerable<WorkItem>> ListAsync(CancellationToken cancellationToken = default);

    public Task<WorkItem?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<WorkItem> CreateAsync(WorkItem workItem, CancellationToken cancellationToken = default);

    public Task<WorkItem> UpdateAsync(Guid id, WorkItem workItem, CancellationToken cancellationToken = default);
}