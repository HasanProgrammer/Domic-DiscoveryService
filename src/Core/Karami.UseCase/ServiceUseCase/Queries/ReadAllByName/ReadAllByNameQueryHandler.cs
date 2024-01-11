using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Service.Contracts.Interfaces;
using Karami.UseCase.ServiceUseCase.DTOs.ViewModels;

namespace Karami.UseCase.ServiceUseCase.Queries.ReadAllByName;

public class ReadAllByNameQueryHandler : IQueryHandler<ReadAllByNameQuery, List<ServicesViewModel>>
{
    private readonly IServiceQueryRepository _serviceQueryRepository;

    public ReadAllByNameQueryHandler(IServiceQueryRepository serviceQueryRepository) 
        => _serviceQueryRepository = serviceQueryRepository; 

    public async Task<List<ServicesViewModel>> HandleAsync(ReadAllByNameQuery byNameQuery, 
        CancellationToken cancellationToken
    )
    {
        var result = await _serviceQueryRepository.FindAllByServiceNameAsync(byNameQuery.ServiceName, cancellationToken);

        return result.Select(service => new ServicesViewModel {
            Name         = service.Name                  ,
            Host         = service.Host                  ,
            IPAddress    = service.IPAddress             ,
            Port         = Convert.ToInt16(service.Port) ,
            ResponseTime = service.ResponseTime          ,
            Status       = service.Status 
        }).ToList();
    }
}