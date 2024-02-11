using Grpc.Core;
using Domic.Core.Service.Grpc;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.ServiceUseCase.Queries.ReadAll;
using Domic.UseCase.ServiceUseCase.Queries.ReadAllByName;
using Domic.UseCase.ServiceUseCase.Queries.ReadOne;
using Domic.WebAPI.Frameworks.Extensions.Mappers.ServiceMappers;

namespace Domic.WebAPI.EntryPoints.GRPCs;

public class ServiceRPC : DiscoveryService.DiscoveryServiceBase
{
    private readonly IMediator      _mediator;
    private readonly IConfiguration _configuration;

    public ServiceRPC(IMediator mediator, IConfiguration configuration)
    {
        _mediator      = mediator;
        _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadOneResponse> ReadOne(ReadOneRequest request, ServerCallContext context)
    {
        var result =
            await _mediator.DispatchAsync(request.ToQuery<ReadOneQuery>(), context.CancellationToken);

        return result.ToRpcResponse<ReadOneResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadAllByNameResponse> ReadAllByName(ReadAllByNameRequest request, 
        ServerCallContext context
    )
    {
        var result =
            await _mediator.DispatchAsync(request.ToQuery<ReadAllByNameQuery>(), context.CancellationToken);

        return result.ToRpcResponse<ReadAllByNameResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadAllResponse> ReadAll(ReadAllRequest request, ServerCallContext context)
    {
        var result =
            await _mediator.DispatchAsync(request.ToQuery<ReadAllQuery>(), context.CancellationToken);

        return result.ToRpcResponse<ReadAllResponse>(_configuration);
    }
}