using Karami.Core.UseCase.DTOs.ViewModels;

namespace Karami.UseCase.ServiceUseCase.DTOs.ViewModels;

public class ServicesViewModel : ViewModel
{
    public string Name      { get; set; }
    public string Host      { get; set; }
    public string IPAddress { get; set; }
    public int Port         { get; set; }
    public int ResponseTime { get; set; }
    public bool Status      { get; set; }
}