using BookShopAppAPI.Models;

namespace BookShopAppAPI.Repositories;

public interface IUserRepository : IRepository
{
    Task AddAsync(AppUser user);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<AppUser?> FindForLoginAsync(string usernameOrEmail);
}
