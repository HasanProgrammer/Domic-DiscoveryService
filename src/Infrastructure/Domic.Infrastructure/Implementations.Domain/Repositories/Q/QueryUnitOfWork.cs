using System.Data;
using Domic.Domain.Commons.Contracts.Interfaces;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class QueryUnitOfWork : IQueryUnitOfWork
{
    public void Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
    }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }

    public void Dispose() {}
}