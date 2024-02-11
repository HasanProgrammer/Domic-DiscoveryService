using Domic.Core.Domain.Enumerations;
using MongoDB.Entities;

namespace Domic.Persistence.Models;

public class ServiceModel : Entity
{
    public string Name         { get; set; }
    public string Host         { get; set; }
    public string IPAddress    { get; set; }
    public int Port            { get; set; }
    public bool Status         { get; set; }
    public int ResponseTime    { get; set; }
    public IsDeleted IsDeleted { get; set; } = IsDeleted.UnDelete;
}