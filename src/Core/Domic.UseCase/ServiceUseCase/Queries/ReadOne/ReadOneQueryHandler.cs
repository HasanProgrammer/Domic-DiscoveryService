using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Service.Contracts.Interfaces;
using Domic.UseCase.ServiceUseCase.DTOs;

namespace Domic.UseCase.ServiceUseCase.Queries.ReadOne;

public class ReadOneQueryHandler : IQueryHandler<ReadOneQuery, ServiceDto>
{
    private readonly IServiceQueryRepository _serviceQueryRepository;

    public ReadOneQueryHandler(IServiceQueryRepository serviceQueryRepository) 
        => _serviceQueryRepository = serviceQueryRepository;

    public async Task<ServiceDto> HandleAsync(ReadOneQuery query, CancellationToken cancellationToken)
    {
        var result = await _serviceQueryRepository.FindAllByServiceNameAsync(query.ServiceName, cancellationToken);
        
        //custom load balance method

        var random = new Random();

        var targetInstanceNumber = random.Next(result.Count);

        var targetInstance = result[targetInstanceNumber];

        return new() {
            Name         = targetInstance.Name                  ,
            Host         = targetInstance.Host                  ,
            IPAddress    = targetInstance.IPAddress             ,
            Port         = Convert.ToInt16(targetInstance.Port) ,
            ResponseTime = targetInstance.ResponseTime          ,
            Status       = targetInstance.Status 
        };
    }
}