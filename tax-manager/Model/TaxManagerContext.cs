using Microsoft.EntityFrameworkCore;

namespace tax_manager.model
{
    public class TaxManagerContext : DbContext
    {
        public TaxManagerContext(DbContextOptions<TaxManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Municipality> Municipalities { get; set; } 
    }
}
