using Domic.UseCase.ServiceUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.ServiceUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<ServicesViewModel>
{
    public string ServiceName { get; set; }
}