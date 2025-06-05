using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Service.Contracts.Interfaces;
using Domic.UseCase.ServiceUseCase.DTOs;

namespace Domic.UseCase.ServiceUseCase.Queries.ReadAllByName;

public class ReadAllByNameQueryHandler : IQueryHandler<ReadAllByNameQuery, List<ServiceDto>>
{
    private readonly IServiceQueryRepository _serviceQueryRepository;

    public ReadAllByNameQueryHandler(IServiceQueryRepository serviceQueryRepository) 
        => _serviceQueryRepository = serviceQueryRepository; 

    public async Task<List<ServiceDto>> HandleAsync(ReadAllByNameQuery byNameQuery, 
        CancellationToken cancellationToken
    )
    {
        var result = await _serviceQueryRepository.FindAllByServiceNameAsync(byNameQuery.ServiceName, cancellationToken);

        return result.Select(service => new ServiceDto {
            Name         = service.Name                  ,
            Host         = service.Host                  ,
            IPAddress    = service.IPAddress             ,
            Port         = Convert.ToInt16(service.Port) ,
            ResponseTime = service.ResponseTime          ,
            Status       = service.Status 
        }).ToList();
    }
}