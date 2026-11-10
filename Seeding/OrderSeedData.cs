using KodaStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaStore.Api.Seeding;

public static class OrderSeedData
{
    private const string DemoOrderNumber = "ORD-001";
    public static void Seed(DbContext context)
    {
        if (context.Set<Order>().Any(o => o.OrderNumber == DemoOrderNumber)) return;

        var keyboard = context.Set<Product>().Single(p => p.ProductCode == "PR001");
        var mouse = context.Set<Product>().Single(p => p.ProductCode == "PR002");

        var order = new Order(DemoOrderNumber);
        order.AddItem(keyboard, 2);
        order.AddItem(mouse, 5);

        context.Set<Order>().Add(order);
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        if (await context.Set<Order>().AnyAsync(o => o.OrderNumber == DemoOrderNumber, cancellationToken)) return;

        var keyboard = await context.Set<Product>().SingleAsync(p => p.ProductCode == "PR001", cancellationToken: cancellationToken);
        var mouse = await context.Set<Product>().SingleAsync(p => p.ProductCode == "PR002", cancellationToken: cancellationToken);

        var order = new Order(DemoOrderNumber);
        order.AddItem(keyboard, 2);
        order.AddItem(mouse, 5);

        context.Set<Order>().Add(order);
        await context.SaveChangesAsync(cancellationToken);
    }
}