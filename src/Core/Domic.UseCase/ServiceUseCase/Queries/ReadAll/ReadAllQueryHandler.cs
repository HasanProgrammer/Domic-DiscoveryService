using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Service.Contracts.Interfaces;
using Domic.UseCase.ServiceUseCase.DTOs;

namespace Domic.UseCase.ServiceUseCase.Queries.ReadAll;

public class ReadAllQueryHandler : IQueryHandler<ReadAllQuery, List<ServiceDto>>
{
    private readonly IServiceQueryRepository _serviceQueryRepository;

    public ReadAllQueryHandler(IServiceQueryRepository serviceQueryRepository)
        => _serviceQueryRepository = serviceQueryRepository;

    public async Task<List<ServiceDto>> HandleAsync(ReadAllQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await _serviceQueryRepository.FindAllAsync(cancellationToken);

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