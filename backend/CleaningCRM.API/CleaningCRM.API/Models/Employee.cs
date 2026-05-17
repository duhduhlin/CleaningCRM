using System.Text.Json.Serialization;

namespace CleaningCRM.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int? UserId { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}