using Grpc.Core;
using Karami.Core.Grpc.Service;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.ServiceUseCase.Queries.ReadAll;
using Karami.UseCase.ServiceUseCase.Queries.ReadAllByName;
using Karami.UseCase.ServiceUseCase.Queries.ReadOne;
using Karami.WebAPI.Frameworks.Extensions.Mappers.ServiceMappers;

namespace Karami.WebAPI.EntryPoints.GRPCs;

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