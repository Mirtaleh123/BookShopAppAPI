using BookShopAppAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (await db.Users.AnyAsync(user => user.Username == "admin")) return;

        var admin = new AppUser
        {
            Username = "admin",
            Email = "admin@bookshop.local",
            PhoneNumber = "0000000000",
            Role = "Admin"
        };
        var password = app.Configuration["AdminSeed:Password"] ?? "admin123";
        admin.PasswordHash = new PasswordHasher<AppUser>().HashPassword(admin, password);
        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }
}
