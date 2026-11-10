using System.ComponentModel;

namespace KodaStore.Api.Models;

public sealed class Order
{
    public long OrderId { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTimeOffset OrderDate { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public ICollection<OrderItem> Items { get; private set; } = [];
    public Order(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new ArgumentException("Order Number wajib diisi.");

        }
        OrderNumber = orderNumber.Trim().ToUpperInvariant();
        OrderDate = DateTimeOffset.UtcNow.AddHours(7);
        Status = "Pending";
    }

    public void AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        if (Status != "Pending") throw new InvalidOperationException("Order tidak bisa diubah setelah konfirmasi");
        if (Items.Any(i => i.ProductCode == product.ProductCode)) throw new ArgumentException("Tidak boleh ada product code yang sama");

        Items.Add(new OrderItem(product, quantity));
    }
}