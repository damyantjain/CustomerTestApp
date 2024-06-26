using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp.Service.Models
{
    /// <summary>
    /// Thhe customer context is used to interact with the database.
    /// </summary>
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }

        /// <summary>
        /// The customers DbSet.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
    }
}
