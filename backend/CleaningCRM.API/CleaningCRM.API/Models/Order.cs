using System;

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

        public Client Client { get; set; }
        public Service Service { get; set; }
        public Employee Employee { get; set; }
    }
}