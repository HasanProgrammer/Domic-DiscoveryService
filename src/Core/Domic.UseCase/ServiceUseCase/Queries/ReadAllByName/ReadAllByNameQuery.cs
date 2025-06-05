using Domic.UseCase.ServiceUseCase.DTOs;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ServiceUseCase.Queries.ReadAllByName;

public class ReadAllByNameQuery : IQuery<List<ServiceDto>>
{
    public string ServiceName { get; set; }
}