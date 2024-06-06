using MongoDB.Entities;

namespace Domic.Persistence.Models;

public class ConsumerEventQueryModel : Entity
{
    public string Type { get; set; }
    public int CountOfRetry { get; set; }
    public DateTime CreatedAt_EnglishDate { get; set; }
    public string CreatedAt_PersianDate { get; set; }
}