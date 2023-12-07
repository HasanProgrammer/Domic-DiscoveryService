using Karami.Core.Common.ClassModels;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Service.Contracts.Interfaces;
using Karami.Domain.Service.Entities;

namespace Karami.UseCase.ServiceUseCase.Events;

public class RegistredServiceConsumerMessageBus : IConsumerMessageBusHandler<ServiceStatus>
{
    private readonly IServiceQueryRepository _serviceQueryRepository;
    
    public RegistredServiceConsumerMessageBus(IServiceQueryRepository serviceQueryRepository) 
        => _serviceQueryRepository = serviceQueryRepository;
    
    public void Handle(ServiceStatus message)
    {
        var targetService =
            _serviceQueryRepository.FindByServiceNameAndIpAddressAsync(message.Name, message.IPAddress, default)
                                   .GetAwaiter()
                                   .GetResult();

        if (targetService is null) //Replication management
        {
            _serviceQueryRepository.Add(new ServiceQuery {
                Name      = message.Name                  ,
                Host      = message.Host                  ,
                IPAddress = message.IPAddress             ,
                Port      = Convert.ToInt16(message.Port) , //todo : tech debt -> must be integere port in [ ServiceStatus]
                Status    = message.Status
            });
        }
    }
}