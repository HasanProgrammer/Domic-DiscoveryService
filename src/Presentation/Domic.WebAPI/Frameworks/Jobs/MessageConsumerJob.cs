using Domic.Core.Common.ClassModels;
using Domic.Core.Domain.Constants;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.WebAPI.Frameworks.Jobs;

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