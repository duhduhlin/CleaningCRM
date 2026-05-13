using System.Text.Json.Serialization;

namespace CleaningCRM.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Address { get; set; }
        public string Status { get; set; } = "Новый";

        // Добавьте [JsonIgnore], чтобы не требовать эти поля в JSON-запросе
        [JsonIgnore]
        public Client? Client { get; set; }
        [JsonIgnore]
        public Service? Service { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}