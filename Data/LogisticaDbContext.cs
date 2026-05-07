using Microsoft.EntityFrameworkCore;

namespace Logistica.API.Data;

public class LogisticaDbContext : DbContext
{
    public LogisticaDbContext(DbContextOptions<LogisticaDbContext> options)
        : base(options) { }

    public DbSet<Products> Products { get; set; }
    public DbSet<Orders> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Products>(entity =>
        {
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("GETDATE()");

     
            entity.HasOne(e => e.Product)
                  .WithMany()
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}