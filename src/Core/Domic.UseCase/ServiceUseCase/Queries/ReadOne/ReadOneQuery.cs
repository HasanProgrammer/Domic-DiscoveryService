using Domic.UseCase.ServiceUseCase.DTOs;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ServiceUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<ServiceDto>
{
    public string ServiceName { get; set; }
}