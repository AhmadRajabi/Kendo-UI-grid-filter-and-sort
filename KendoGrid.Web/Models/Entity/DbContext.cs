using Microsoft.EntityFrameworkCore;

namespace KendoGrid.Web.Models.Entity
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
  
    }
}
