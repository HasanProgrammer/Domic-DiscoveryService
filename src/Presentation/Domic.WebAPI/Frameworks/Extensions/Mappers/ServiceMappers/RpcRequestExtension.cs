using Domic.Core.Service.Grpc;
using Domic.UseCase.ServiceUseCase.Queries.ReadAll;
using Domic.UseCase.ServiceUseCase.Queries.ReadAllByName;
using Domic.UseCase.ServiceUseCase.Queries.ReadOne;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.ServiceMappers;

//Query
public static partial class RpcRequestExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToQuery<T>(this ReadOneRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ReadOneQuery))
            Request = new ReadOneQuery {
                ServiceName = request.Name?.Value
            };

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToQuery<T>(this ReadAllByNameRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ReadAllByNameQuery))
            Request = new ReadAllByNameQuery {
                ServiceName = request.Name.Value
            };

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToQuery<T>(this ReadAllRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ReadAllQuery))
            Request = new ReadAllQuery();

        return (T)Request;
    }
}

//Command
public partial class RpcRequestExtension
{
    
}