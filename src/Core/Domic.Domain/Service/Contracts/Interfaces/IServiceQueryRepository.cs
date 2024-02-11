using Domic.Domain.Service.Entities;
using Domic.Core.Domain.Contracts.Interfaces;

namespace Domic.Domain.Service.Contracts.Interfaces;

public interface IServiceQueryRepository : IQueryRepository<ServiceQuery, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="ip"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<ServiceQuery> FindByServiceNameAndIpAddressAsync(string service, string ip, 
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<ServiceQuery>> FindAllByServiceNameAsync(string service, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}