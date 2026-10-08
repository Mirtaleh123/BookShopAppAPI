using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories;

public class UserRepository(ApplicationDbContext db) : BaseRepository<AppUser>(db), IUserRepository
{
    public Task<bool> UsernameExistsAsync(string username) =>
        Entities.AnyAsync(user => user.Username == username);

    public Task<bool> EmailExistsAsync(string email) =>
        Entities.AnyAsync(user => user.Email == email);

    public Task<AppUser?> FindForLoginAsync(string usernameOrEmail) =>
        Entities.FirstOrDefaultAsync(user =>
            user.Username == usernameOrEmail || user.Email == usernameOrEmail);
}
