using System.ComponentModel.DataAnnotations;

namespace BookShopAppAPI.Contracts;

public sealed class CreateBookRequest
{
    [Required, StringLength(200)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Author { get; set; } = string.Empty;
    [Range(0.01, 10000)] public decimal Price { get; set; }
    [Required] public string ImageUrl { get; set; } = string.Empty;
    [Required, StringLength(1000)] public string About { get; set; } = string.Empty;
    [Range(0, int.MaxValue)] public int Stock { get; set; }
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
}

public sealed class UpdateBookRequest
{
    [Required, StringLength(200)] public string Title { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Author { get; set; } = string.Empty;
    [Range(0.01, 10000)] public decimal Price { get; set; }
    [Required] public string ImageUrl { get; set; } = string.Empty;
    [Required, StringLength(1000)] public string About { get; set; } = string.Empty;
    [Range(0, int.MaxValue)] public int Stock { get; set; }
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
}

public sealed class RegisterRequest
{
    [Required, MinLength(3)] public string Username { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string PhoneNumber { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    [Required, Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class LoginRequest
{
    [Required] public string UsernameOrEmail { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public sealed class RateBookRequest
{
    [Range(1, 5)] public int Rating { get; set; }
}

public sealed class AddCartItemRequest
{
    [Range(1, int.MaxValue)] public int BookId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1;
}

public sealed class UpdateOrderStatusRequest
{
    [Required] public string Status { get; set; } = string.Empty;
}
