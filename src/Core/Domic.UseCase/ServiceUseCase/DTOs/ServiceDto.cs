namespace Domic.UseCase.ServiceUseCase.DTOs;

public class ServiceDto
{
    public string Name      { get; set; }
    public string Host      { get; set; }
    public string IPAddress { get; set; }
    public int Port         { get; set; }
    public int ResponseTime { get; set; }
    public bool Status      { get; set; }
}