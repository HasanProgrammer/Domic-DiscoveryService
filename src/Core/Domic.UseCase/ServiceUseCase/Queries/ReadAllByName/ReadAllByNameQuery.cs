using Domic.UseCase.ServiceUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ServiceUseCase.Queries.ReadAllByName;

public class ReadAllByNameQuery : IQuery<List<ServicesViewModel>>
{
    public string ServiceName { get; set; }
}