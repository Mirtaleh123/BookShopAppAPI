namespace BookShopAppAPI.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Order Order { get; set; } = null!;
    public Book Book { get; set; } = null!;
}

public enum OrderStatus
{
    Pending,
    Approved,
    Delivered,
    Cancelled
}
