using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories;

public interface IUserRepository
{
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<AppUser?> FindForLoginAsync(string usernameOrEmail);
    Task AddAsync(AppUser user);
    Task SaveAsync();
}

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public Task<bool> UsernameExistsAsync(string username) =>
        db.Users.AnyAsync(user => user.Username == username);

    public Task<bool> EmailExistsAsync(string email) =>
        db.Users.AnyAsync(user => user.Email == email);

    public Task<AppUser?> FindForLoginAsync(string usernameOrEmail) =>
        db.Users.FirstOrDefaultAsync(user =>
            user.Username == usernameOrEmail || user.Email == usernameOrEmail);

    public async Task AddAsync(AppUser user) => await db.Users.AddAsync(user);
    public async Task SaveAsync() => await db.SaveChangesAsync();
}
