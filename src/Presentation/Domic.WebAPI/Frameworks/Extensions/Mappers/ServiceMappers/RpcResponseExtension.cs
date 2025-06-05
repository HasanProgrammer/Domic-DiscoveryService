using Domic.Core.Common.ClassExtensions;
using Domic.Core.Service.Grpc;
using Domic.UseCase.ServiceUseCase.DTOs;

using String = Domic.Core.Service.Grpc.String;
using Int32  = Domic.Core.Service.Grpc.Int32;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.ServiceMappers;

//Query
public static partial class RpcResponseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this ServiceDto model, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadOneResponse))
        {
            Response = new ReadOneResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body = new ReadOneResponseBody {
                    Service = new Service
                    {
                        Host      = new String { Value = model.Host }      ,
                        IpAddress = new String { Value = model.IPAddress } ,
                        Port      = new Int32  { Value = model.Port }
                    }
                }
            };
        }

        return (T) Response;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="models"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this List<ServiceDto> models, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadAllByNameResponse))
        {
            var body = new ReadAllByNameResponseBody();
            
            body.Services.AddRange(
                models.Select(model => new Service {
                    Host         = new String { Value = model.Host }         ,
                    IpAddress    = new String { Value = model.IPAddress }    ,
                    Port         = new Int32  { Value = model.Port }         ,
                    Name         = new String { Value = model.Name }         ,
                    ResponseTime = new Int32  { Value = model.ResponseTime } ,
                    Status       = model.Status 
                })
            );
            
            Response = new ReadAllByNameResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body    = body
            };
        }
        else if (typeof(T) == typeof(ReadAllResponse))
        {
            var body = new ReadAllResponseBody();
            
            body.Services.AddRange(
                models.Select(model => new Service {
                    Host         = new String { Value = model.Host }         ,
                    IpAddress    = new String { Value = model.IPAddress }    ,
                    Port         = new Int32  { Value = model.Port }         ,
                    Name         = new String { Value = model.Name }         ,
                    ResponseTime = new Int32  { Value = model.ResponseTime } ,
                    Status       = model.Status 
                })
            );
            
            Response = new ReadAllResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body    = body
            };
        }

        return (T) Response;
    }
}

//Command
public partial class RpcResponseExtension
{
    
}