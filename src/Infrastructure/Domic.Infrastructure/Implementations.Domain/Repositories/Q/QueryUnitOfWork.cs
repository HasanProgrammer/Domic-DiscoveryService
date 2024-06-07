#pragma warning disable CS0169 // Field is never used

using System.Data;
using Domic.Domain.Commons.Contracts.Interfaces;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class QueryUnitOfWork(DBContext dbContext) : IQueryUnitOfWork
{
    private IClientSessionHandle _transaction;

    public void Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        #if false
            _transaction = dbContext.Transaction();
        #endif
    }

    public Task TransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = new CancellationToken())
    {
        #if false
            _transaction = dbContext.Transaction();
        #endif

        return Task.CompletedTask;
    }

    public void Commit()
    {
        #if false
            _transaction?.CommitTransaction();
        #endif
    }
    
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        #if false
            if (_transaction is not null)
                return _transaction.CommitTransactionAsync(cancellationToken);
        #endif

        return Task.CompletedTask;
    }

    public void Rollback()
    {
        #if false
            _transaction?.AbortTransaction();
        #endif
    }

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        #if false
            if (_transaction is not null)
                return _transaction.AbortTransactionAsync(cancellationToken);
        #endif

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        #if false
            _transaction?.Dispose();
        #endif
    }
    
    public ValueTask DisposeAsync()
    {
        #if false
            _transaction?.Dispose();
        #endif
        
        return ValueTask.CompletedTask;
    }
}