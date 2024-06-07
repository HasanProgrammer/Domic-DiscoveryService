using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Entities;
using Domic.Persistence.Models;
using MongoDB.Entities;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class ConsumerEventQueryRepository(DBContext dbContext) : IConsumerEventQueryRepository
{
    public ConsumerEventQuery FindById(object id)
    {
        var result =
            dbContext.Find<ConsumerEventQueryModel>().Match(s => s.ID == id as string).ExecuteFirstAsync().Result;

        if (result is null) return null;

        return new();
    }

    public async Task<ConsumerEventQuery> FindByIdAsync(object id, CancellationToken cancellationToken)
    {
        var result =
            await dbContext.Find<ConsumerEventQueryModel>()
                            .Match(s => s.ID == id as string)
                            .ExecuteFirstAsync(cancellationToken);
        
        if (result is null) return null;

        return new();
    }

    public void Add(ConsumerEventQuery entity)
    {
        var newModel = new ConsumerEventQueryModel {
            ID = entity.Id,
            Type = entity.Type,
            CountOfRetry = entity.CountOfRetry,
            CreatedAt_EnglishDate = entity.CreatedAt_EnglishDate,
            CreatedAt_PersianDate = entity.CreatedAt_PersianDate
        };
        
        dbContext.SaveAsync(newModel).GetAwaiter().GetResult();
    }
}