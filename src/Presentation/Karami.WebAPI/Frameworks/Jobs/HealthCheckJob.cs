using System.Diagnostics;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Extensions;
using Karami.Domain.Service.Contracts.Interfaces;

namespace Karami.WebAPI.Frameworks.Jobs;

public class HealthCheckJob : BackgroundService
{
    private readonly IHostEnvironment     _hostEnvironment;
    private readonly IConfiguration       _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HealthCheckJob(IServiceScopeFactory serviceScopeFactory, IHostEnvironment hostEnvironment,
        IConfiguration configuration
    )
    {
        _hostEnvironment     = hostEnvironment;
        _configuration       = configuration;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var serviceQueryRepository = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
            var dotrisDateTime         = scope.ServiceProvider.GetRequiredService<IDotrisDateTime>();
            var messageBroker          = scope.ServiceProvider.GetRequiredService<IMessageBroker>();

            try
            {
                foreach (var targetService in await serviceQueryRepository.FindAllAsync(stoppingToken))
                {
                    //If target service is unreachable ( status = false ) must be removed

                    if (!targetService.Status)
                        await serviceQueryRepository.RemoveAsync(targetService.Id, stoppingToken);

                    var stopWatch = new Stopwatch();
                
                    try
                    {
                        //Send request to target service ( health-check ) route
                        
                        using var httpClient = new HttpClient(new HttpClientHandler {
                            ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
                        });

                        var endpoint = _hostEnvironment.IsProduction() ? targetService.IPAddress : targetService.Host; 

                        stopWatch.Start();

                        var httpResponseMessage =
                            await httpClient.GetAsync($"https://{endpoint}:{targetService.Port}/health",
                                stoppingToken
                            );
                        
                        stopWatch.Stop();
                    
                        targetService.Status = httpResponseMessage.IsSuccessStatusCode;
                    }
                    catch (Exception e)
                    {
                        //for debug
                        e.FileLogger(_hostEnvironment, dotrisDateTime);
                        
                        targetService.Status = false;
                    }
                    finally
                    {
                        targetService.ResponseTime = (int)stopWatch.Elapsed.TotalSeconds;
                        await serviceQueryRepository.ChangeAsync(targetService, stoppingToken);
                    }
                }
            }
            catch (Exception e)
            {
                e.FileLogger(_hostEnvironment, dotrisDateTime);
                e.CentralExceptionLogger(_hostEnvironment, messageBroker, dotrisDateTime, 
                    _configuration.GetValue<string>("NameOfService"), nameof(MessageConsumerJob)
                );
            }
            
            //60s wait
            await Task.Delay(60000);
        }
    }
}