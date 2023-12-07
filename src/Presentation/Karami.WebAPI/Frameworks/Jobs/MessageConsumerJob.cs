using Karami.Core.Common.ClassModels;
using Karami.Core.Domain.Constants;
using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.WebAPI.Frameworks.Jobs;

public class MessageConsumerJob : IHostedService
{
    private readonly IMessageBroker _messageBroker;
    private readonly IConfiguration _configuration;

    public MessageConsumerJob(IMessageBroker messageBroker, IConfiguration configuration)
    {
        _messageBroker = messageBroker;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messageBroker.NameOfAction  = nameof(MessageConsumerJob);
        _messageBroker.NameOfService = _configuration.GetValue<string>("NameOfService");

        _messageBroker.Subscribe<ServiceStatus>(Broker.ServiceRegistry_Queue);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}