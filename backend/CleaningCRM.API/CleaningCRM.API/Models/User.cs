using System.Text.Json.Serialization;

namespace CleaningCRM.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public int? EmployeeId { get; set; }
        public int? ClientId { get; set; }

        // НЕТ навигационных свойств!
    }
}