namespace BookShopAppAPI.Models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; } = 1;
    public Book Book { get; set; } = null!;
}
