using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.ServiceUseCase.DTOs.ViewModels;

namespace Karami.UseCase.ServiceUseCase.Queries.ReadAllByName;

public class ReadAllByNameQuery : IQuery<List<ServicesViewModel>>
{
    public string ServiceName { get; set; }
}