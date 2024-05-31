using Domic.Core.Common.ClassConsts;
using Domic.Core.Common.ClassModels;
using Domic.Core.Domain.Constants;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Commons.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Service.Contracts.Interfaces;
using Domic.Domain.Service.Entities;

namespace Domic.UseCase.ServiceUseCase.Events;

[Consumer(Queue = Broker.ServiceRegistry_Queue)]
public class RegistredServiceConsumerMessageBus : IConsumerMessageBusHandler<ServiceStatus>
{
    private readonly IServiceQueryRepository _serviceQueryRepository;
    
    public RegistredServiceConsumerMessageBus(IServiceQueryRepository serviceQueryRepository) 
        => _serviceQueryRepository = serviceQueryRepository;

    [TransactionConfig(Type = TransactionType.Query)]
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

    [TransactionConfig(Type = TransactionType.Query)]
    public async Task HandleAsync(ServiceStatus message, CancellationToken cancellationToken)
    {
        var targetService =
            await _serviceQueryRepository.FindByServiceNameAndIpAddressAsync(message.Name, message.IPAddress, 
                cancellationToken
            );

        if (targetService is null) //replication management
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