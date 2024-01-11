using Karami.Core.Common.ClassExtensions;
using Karami.Domain.Service.Contracts.Interfaces;
using Karami.Domain.Service.Entities;
using Karami.Persistence.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Karami.Infrastructure.Implementations.Domain.Repositories.Q;

public class ServiceQueryRepository : IServiceQueryRepository
{
    private readonly DBContext _dbContext;
    
    public ServiceQueryRepository(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        var connection = configuration.GetMongoConnectionString();

        _dbContext = new DBContext("ServiceRegistry", MongoClientSettings.FromConnectionString(connection));
    }

    public void Add(ServiceQuery entity)
    {
        //Should use mapper
        var serviceModel = new ServiceModel {
            Name      = entity.Name      ,
            Host      = entity.Host      ,
            IPAddress = entity.IPAddress ,
            Port      = entity.Port      ,
            Status    = entity.Status
        };

        _dbContext.SaveAsync(serviceModel).GetAwaiter().GetResult();
    }

    public async Task ChangeAsync(ServiceQuery entity, CancellationToken cancellationToken)
    {
        await _dbContext.Update<ServiceModel>()
                        .MatchID(entity.Id)
                        .Modify(order => 
                            order.Set(s => s.ResponseTime, entity.ResponseTime)
                                 .Set(s => s.Status, entity.Status)
                        )
                        .ExecuteAsync(cancellationToken);
    }

    public async Task RemoveAsync(object id, CancellationToken cancellationToken)
        => await _dbContext.DeleteAsync<ServiceModel>(model => model.ID.Equals(id));

    public async Task<IEnumerable<ServiceQuery>> FindAllAsync(CancellationToken cancellationToken)
    {
        var targetServices =
            await _dbContext.Find<ServiceModel, ServiceQuery>()
                            .Match(s => s.Status == true)
                            .Project(s => new ServiceQuery {
                                Id           = s.ID        ,
                                Name         = s.Name      ,
                                Host         = s.Host      ,
                                IPAddress    = s.IPAddress ,
                                Port         = s.Port      ,
                                Status       = s.Status    ,
                                ResponseTime = s.ResponseTime
                            })
                            .ExecuteAsync(cancellationToken);
        
        return targetServices;
    }
    
    public async Task<ServiceQuery> FindByServiceNameAndIpAddressAsync(string service, string ip, 
        CancellationToken cancellationToken
    )
    {
        var targetService =
            await _dbContext.Find<ServiceModel, ServiceQuery>()
                            .Match(s => s.Name.Equals(service) && s.IPAddress.Equals(ip) && s.Status == true)
                            .Project(s => new ServiceQuery {
                                Id        = s.ID        ,
                                Name      = s.Name      ,
                                Host      = s.Host      ,
                                IPAddress = s.IPAddress ,
                                Port      = s.Port      ,
                                Status    = s.Status
                            })
                            .ExecuteFirstAsync(cancellationToken);
        
        return targetService;
    }

    public async Task<List<ServiceQuery>> FindAllByServiceNameAsync(string service, CancellationToken cancellationToken)
    {
        var targetServices =
            await _dbContext.Find<ServiceModel, ServiceQuery>()
                            .Match(s => s.Name.Equals(service) && s.Status == true)
                            .Project(s => new ServiceQuery {
                                Id        = s.ID        ,
                                Name      = s.Name      ,
                                Host      = s.Host      ,
                                IPAddress = s.IPAddress ,
                                Port      = s.Port      ,
                                Status    = s.Status
                            })
                            .ExecuteAsync(cancellationToken);
        
        return targetServices;
    }
}