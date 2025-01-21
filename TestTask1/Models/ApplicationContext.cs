using Microsoft.EntityFrameworkCore;

namespace TestTask1.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Provider> Providers { get; set; } = null!;

       public ApplicationContext(DbContextOptions<ApplicationContext> options)
       : base(options)
        {
        }
    }
}
