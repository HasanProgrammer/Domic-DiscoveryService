using Domic.Core.Common.ClassExtensions;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Entities;
using Domic.Persistence.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class ConsumerEventQueryRepository : IConsumerEventQueryRepository
{
    private readonly DBContext _dbContext;
    
    public ConsumerEventQueryRepository(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        var connection = configuration.GetMongoConnectionString();

        _dbContext = new DBContext("ConsumerEventQuery", MongoClientSettings.FromConnectionString(connection));
    }

    public ConsumerEventQuery FindById(object id)
        => _dbContext.Find<ConsumerEventQueryModel, ConsumerEventQuery>()
                     .Match(s => s.ID.Equals(id.ToString()))
                     .Project(s => new ConsumerEventQuery {
                         Id = s.ID,
                         Type = s.Type,
                         CountOfRetry = s.CountOfRetry,
                         CreatedAt_EnglishDate = s.CreatedAt_EnglishDate,
                         CreatedAt_PersianDate = s.CreatedAt_PersianDate
                     })
                     .ExecuteFirstAsync().Result;
    
    public Task<ConsumerEventQuery> FindByIdAsync(object id, CancellationToken cancellationToken) 
        => _dbContext.Find<ConsumerEventQueryModel, ConsumerEventQuery>()
                     .Match(s => s.ID.Equals(id.ToString()))
                     .Project(s => new ConsumerEventQuery {
                         Id = s.ID,
                         Type = s.Type,
                         CountOfRetry = s.CountOfRetry,
                         CreatedAt_EnglishDate = s.CreatedAt_EnglishDate,
                         CreatedAt_PersianDate = s.CreatedAt_PersianDate
                     })
                     .ExecuteFirstAsync(cancellationToken);

    public void Add(ConsumerEventQuery entity)
    {
        var newModel = new ConsumerEventQueryModel {
            ID = entity.Id,
            Type = entity.Type,
            CountOfRetry = entity.CountOfRetry,
            CreatedAt_EnglishDate = entity.CreatedAt_EnglishDate,
            CreatedAt_PersianDate = entity.CreatedAt_PersianDate
        };
        
        _dbContext.SaveAsync(newModel).GetAwaiter().GetResult();
    }
}