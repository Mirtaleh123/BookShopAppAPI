using System.ComponentModel.DataAnnotations;

namespace BookShopAppAPI.Models;

public class BookRating
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    [Range(1, 5)] public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
