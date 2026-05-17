using System.Text.Json.Serialization;

namespace CleaningCRM.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public int? EmployeeId { get; set; }

        // Временно удалите эти строки:
        // [JsonIgnore]
        // public Employee? Employee { get; set; }
    }
}