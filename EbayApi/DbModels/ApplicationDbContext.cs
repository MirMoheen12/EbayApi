using Microsoft.EntityFrameworkCore;

namespace EbayApi.DbModels
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AllUsers> AllUsers { get; set; }
      
    }
}
