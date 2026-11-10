
using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Seeding;

public static class ProductSeedData
{
    private static IEnumerable<Product> InitialProduct(int customerId)
    {
        for (int i = 1; i <= 10; i++)
        {
            string productCode = $"PR{i:D3}";
            string productName = $"Product Dummy {i}";

            long price = 15000L + (i * 1000L);
            int stock = 20;

            string category = i % 3 == 0
                ? "Elektronik"
                : (i % 2 == 0) ? "Mouse" : "Screen";

            yield return new Product(
                customerId,
                productCode,
                productName,
                price,
                stock,
                category
            );
        }
    }

    public static void Seed(DbContext context)
    {
        var customer = context.Set<Customer>()
            .Single(c => c.Email == CustomerSeedData.DemoEmail);

        foreach (var product in InitialProduct(customer.Id))
        {
            bool exists = context.Set<Product>()
                .Any(p => p.ProductCode == product.ProductCode);

            if (!exists)
            {
                context.Set<Product>().Add(product);
            }
        }

        context.SaveChanges();
    }

    public static async Task SeedAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        var customer = await context.Set<Customer>()
            .SingleAsync(
                c => c.Email == CustomerSeedData.DemoEmail,
                cancellationToken
            );

        foreach (var product in InitialProduct(customer.Id))
        {
            bool exists = await context.Set<Product>()
                .AnyAsync(
                    p => p.ProductCode == product.ProductCode,
                    cancellationToken
                );

            if (!exists)
            {
                context.Set<Product>().Add(product);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
