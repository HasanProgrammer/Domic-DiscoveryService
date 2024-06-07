using System.Data;
using Domic.Core.Common.ClassExtensions;
using Domic.Domain.Commons.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class QueryUnitOfWork : IQueryUnitOfWork
{
    private DBContext _dbContext;
    private IClientSessionHandle _transaction;

    public QueryUnitOfWork(IConfiguration configuration)
    {
        var connection = configuration.GetMongoConnectionString();

        _dbContext = new DBContext("ServiceRegistry", MongoClientSettings.FromConnectionString(connection));
    }
    
    public void Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        => _transaction = _dbContext.Transaction();

    public Task TransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _transaction = _dbContext.Transaction();

        return Task.CompletedTask;
    }

    public void Commit() => _transaction?.CommitTransaction();
    
    public Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
            return _transaction.CommitTransactionAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public void Rollback() => _transaction?.AbortTransaction();

    public Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
            return _transaction.AbortTransactionAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public void Dispose() => _transaction?.Dispose();
    
    public ValueTask DisposeAsync()
    {
        _transaction?.Dispose();
        
        return ValueTask.CompletedTask;
    }
}