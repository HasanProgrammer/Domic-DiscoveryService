using Domic.Domain.Service.Contracts.Interfaces;
using Domic.Domain.Service.Entities;
using Domic.Persistence.Models;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Domic.Infrastructure.Implementations.Domain.Repositories.Q;

public class ServiceQueryRepository(DBContext dbContext) : IServiceQueryRepository
{
    public void Add(ServiceQuery entity)
    {
        var serviceModel = new ServiceModel {
            Name      = entity.Name      ,
            Host      = entity.Host      ,
            IPAddress = entity.IPAddress ,
            Port      = entity.Port      ,
            Status    = entity.Status
        };

        dbContext.SaveAsync(serviceModel).GetAwaiter().GetResult();
    }

    public async Task ChangeAsync(ServiceQuery entity, CancellationToken cancellationToken)
    {
        await dbContext.Update<ServiceModel>()
                       .MatchID(entity.Id)
                       .Modify(order => 
                           order.Set(s => s.ResponseTime, entity.ResponseTime)
                                .Set(s => s.Status, entity.Status)
                       )
                       .ExecuteAsync(cancellationToken);
    }

    public async Task RemoveAsync(object id, CancellationToken cancellationToken)
        => await dbContext.DeleteAsync<ServiceModel>(model => model.ID.Equals(id));

    public async Task<IEnumerable<ServiceQuery>> FindAllAsync(CancellationToken cancellationToken)
    {
        var targetServices =
            await dbContext.Find<ServiceModel, ServiceQuery>()
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
            await dbContext.Find<ServiceModel, ServiceQuery>()
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
            await dbContext.Find<ServiceModel, ServiceQuery>()
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