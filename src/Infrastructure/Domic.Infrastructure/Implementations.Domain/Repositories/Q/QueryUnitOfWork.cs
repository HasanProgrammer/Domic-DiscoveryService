using System.Data;
using Domic.Domain.Commons.Contracts.Interfaces;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class QueryUnitOfWork : IQueryUnitOfWork
{
    public void Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
    }

    public Task TransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.CompletedTask;
    }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }

    public void Dispose() {}
}