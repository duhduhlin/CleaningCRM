using CleaningCRM.API.Models;
using Microsoft.EntityFrameworkCore;  // ЭТО САМОЕ ВАЖНОЕ!
using System.Collections.Generic;

namespace CleaningCRM.API.Data
{
    public class AppDbContext : DbContext  // ← ДОЛЖНО БЫТЬ : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}