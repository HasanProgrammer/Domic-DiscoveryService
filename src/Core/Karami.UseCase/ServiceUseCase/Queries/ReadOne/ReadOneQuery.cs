using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.ServiceUseCase.DTOs.ViewModels;

namespace Karami.UseCase.ServiceUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<ServicesViewModel>
{
    public string ServiceName { get; set; }
}