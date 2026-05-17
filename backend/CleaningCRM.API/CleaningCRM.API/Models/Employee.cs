using System.Text.Json.Serialization;

namespace CleaningCRM.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public int? UserId { get; set; }

        // Временно удалите эти строки:
        // [JsonIgnore]
        // public User? User { get; set; }

        // [JsonIgnore]
        // public ICollection<Order>? Orders { get; set; }
    }
}