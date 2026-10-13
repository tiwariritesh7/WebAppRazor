namespace WebAppRazor.ORM
{
    using Microsoft.EntityFrameworkCore;
    using WebAppRazor.DTO;

    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<ProductDTO> products { get; set; }
        public DbSet<CustomerDTO> customers { get; set; }
        public DbSet<CustomerProductsDTO> customersProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDTO>()
                .Property(p => p.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<ProductDTO>(e =>
            {
                e.ToTable("Products");
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).IsRequired();
                // additional property configuration if needed
            });

            modelBuilder.Entity<CustomerDTO>(e =>
            {
                e.ToTable("Customers");
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired();
            });

            modelBuilder.Entity<CustomerProductsDTO>(e =>
            {
                e.ToTable("CustomerProducts");
                e.HasKey(cp => cp.Id);
                e.Property(cp => cp.ProductId).IsRequired();
                e.HasOne<ProductDTO>()
                 .WithMany()                      // add navigation if needed and change this
                 .HasForeignKey(cp => cp.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
