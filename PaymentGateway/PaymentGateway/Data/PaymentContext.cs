using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Models
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {

        }

        public DbSet<PaymentItem> PaymentItems { get; set; }
    } 
}
