
using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Seeding;

public static class CustomerSeedData
{
    public const string DemoEmail = "demo@kodastore.com";

    public static void Seed(DbContext context)
    {
        bool exists = context.Set<Customer>()
            .Any(c => c.Email == DemoEmail);

        if (exists) return;

        var customer = new Customer(
            "Demo Customer",
            DemoEmail,
            "081234567890",
            "Jakarta, Indonesia"
        );

        context.Set<Customer>().Add(customer);
        context.SaveChanges();
    }

    public static async Task SeedAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        bool exists = await context.Set<Customer>()
            .AnyAsync(c => c.Email == DemoEmail, cancellationToken);

        if (exists) return;

        var customer = new Customer(
            "Demo Customer",
            DemoEmail,
            "081234567890",
            "Jakarta, Indonesia"
        );

        context.Set<Customer>().Add(customer);
        await context.SaveChangesAsync(cancellationToken);
    }
}
