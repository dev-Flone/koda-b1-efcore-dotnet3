using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", "dbo", table =>
            {
                table.HasCheckConstraint("CK_Product_Price", "[Price] >= 0");
                table.HasCheckConstraint("CK_Product_Stock", "[Stock] >= 0");
            });

            entity.HasKey(p => p.ProductCode);
            entity.Property(p => p.ProductCode).HasMaxLength(30).IsRequired().ValueGeneratedNever();
            entity.Property(p => p.Name).HasMaxLength(120).IsRequired();
            entity.Property(p => p.Price).IsRequired();
            entity.Property(p => p.Stock).IsRequired();
            entity.Property(p => p.Category).HasMaxLength(30).IsRequired();

            entity.HasOne(p => p.Customer).WithMany(c => c.Products).HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer", "dbo");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name).HasMaxLength(120).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(150).IsRequired();
            entity.Property(c => c.Phone).HasMaxLength(15).IsRequired();
            entity.Property(c => c.Address).HasMaxLength(255).IsRequired();

            entity.Navigation(c => c.Products).HasField("_products").UsePropertyAccessMode(PropertyAccessMode.Field);

        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders", "dbo", table => table.HasCheckConstraint("CK_Orders_Status", "[Status] in ('Pending', 'Confirmed')"));
            entity.HasKey(o => o.OrderId);
            entity.Property(o => o.OrderId).ValueGeneratedOnAdd();
            entity.Property(o => o.OrderNumber).HasMaxLength(35).IsRequired();
            entity.Property(o => o.OrderDate).HasColumnType("datetimeoffset").IsRequired();
            entity.Property(o => o.Status).HasMaxLength(20).IsRequired();
            entity.HasIndex(o => o.OrderDate);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem", "dbo", table =>
            {
                table.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0");
                table.HasCheckConstraint("CK_OrderItem_UnitPrice", "[UnitPrice] >= 0");
            });
            entity.HasKey(i => i.OrderItemId);
            entity.Property(i => i.OrderItemId).ValueGeneratedOnAdd();
            entity.Property(i => i.ProductCode).HasMaxLength(30).IsRequired();
            entity.Property(i => i.Quantity).IsRequired();
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
            entity.Property(i => i.SubTotal).HasPrecision(18, 2).HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);

            entity.HasOne(i => i.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(i => i.ProductCode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(i => new { i.OrderId, i.ProductCode }).IsUnique();
            entity.HasIndex(i => i.ProductCode);
        });
    }
}