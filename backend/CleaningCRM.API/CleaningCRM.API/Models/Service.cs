using System.Text.Json.Serialization;

namespace CleaningCRM.API.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // НАВИГАЦИОННОЕ СВОЙСТВО (ДОБАВИТЬ)
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}