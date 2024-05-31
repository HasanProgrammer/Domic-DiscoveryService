using System.Diagnostics;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Infrastructure.Extensions;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Service.Contracts.Interfaces;

namespace Domic.WebAPI.Frameworks.Jobs;

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

            var serviceQueryRepository   = scope.ServiceProvider.GetRequiredService<IServiceQueryRepository>();
            var dateTime                 = scope.ServiceProvider.GetRequiredService<IDateTime>();
            var messageBroker            = scope.ServiceProvider.GetRequiredService<IMessageBroker>();
            var globalUniqueIdGenerator  = scope.ServiceProvider.GetRequiredService<IGlobalUniqueIdGenerator>();
            var externalDistributedCache = scope.ServiceProvider.GetRequiredService<IExternalDistributedCache>();
            var serializer               = scope.ServiceProvider.GetRequiredService<ISerializer>();

            try
            {
                foreach (var targetService in await serviceQueryRepository.FindAllAsync(stoppingToken))
                {
                    #if false

                    //if target service is unreachable ( status = false ) must be removed

                    if (!targetService.Status)
                        await serviceQueryRepository.RemoveAsync(targetService.Id, stoppingToken);
                    
                    #endif

                    var stopWatch = new Stopwatch();
                
                    try
                    {
                        //send request to target service ( health-check ) route
                        
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
                        e.FileLogger(_hostEnvironment, dateTime);
                        
                        targetService.Status = false;
                    }
                    finally
                    {
                        targetService.ResponseTime = (int)stopWatch.Elapsed.TotalSeconds;
                        
                        await serviceQueryRepository.ChangeAsync(targetService, stoppingToken);
                    }
                }
                
                #region DistributedCachedServicesInfoProcessing
                
                var services = await serviceQueryRepository.FindAllAsync(stoppingToken);
                
                await externalDistributedCache.SetCacheValueAsync(
                    new KeyValuePair<string, string>("ServicesInfo", serializer.Serialize(services)),
                    cancellationToken: stoppingToken
                );

                #endregion
            }
            catch (Exception e)
            {
                var serviceName = _configuration.GetValue<string>("NameOfService");
                
                e.FileLogger(_hostEnvironment, dateTime);
                
                e.ElasticStackExceptionLogger(_hostEnvironment, globalUniqueIdGenerator, dateTime, serviceName, 
                    nameof(HealthCheckJob)
                );
                
                e.CentralExceptionLogger(_hostEnvironment, globalUniqueIdGenerator, messageBroker, dateTime, 
                    serviceName, nameof(HealthCheckJob)
                );
            }
            
            //60s wait
            await Task.Delay(60000);
        }
    }
}