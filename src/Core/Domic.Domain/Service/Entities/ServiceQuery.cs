#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.Service.Entities;

public class ServiceQuery : BaseEntityQuery<string>
{
    //Fields
    
    public string Name      { get; set; }
    public string Host      { get; set; }
    public string IPAddress { get; set; }
    public int Port         { get; set; }
    public bool Status      { get; set; }
    public int ResponseTime { get; set; }

    /*---------------------------------------------------------------*/
    
    //Relations
}