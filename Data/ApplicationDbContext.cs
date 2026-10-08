using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<BookRating> BookRatings => Set<BookRating>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().Property(book => book.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(order => order.TotalPrice).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(item => item.Price).HasPrecision(18, 2);

        modelBuilder.Entity<BookRating>()
            .HasIndex(rating => new { rating.BookId, rating.UserId })
            .IsUnique();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Tarix" },
            new Category { Id = 2, Name = "Roman" },
            new Category { Id = 3, Name = "Psixologiya" },
            new Category { Id = 4, Name = "Fəlsəfə" },
            new Category { Id = 5, Name = "Elm" });

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Elçibəy", Author = "Mehmet Necati Dəmircan", Price = 11.82m, Stock = 10, CategoryId = 1, ImageUrl = "https://example.com/elcibey.jpg", About = "Tarixi roman." },
            new Book { Id = 2, Title = "İkigai", Author = "Bettina Lemke", Price = 8.49m, Stock = 10, CategoryId = 3, ImageUrl = "https://example.com/ikigai.jpg", About = "Həyatı mənalandırmaq haqqında kitab." },
            new Book { Id = 3, Title = "Üç yoldaş", Author = "Erix Mariya Remark", Price = 11.04m, Stock = 10, CategoryId = 2, ImageUrl = "https://example.com/uc-yoldas.jpg", About = "Dostluq və həyat haqqında roman." });
    }
}
