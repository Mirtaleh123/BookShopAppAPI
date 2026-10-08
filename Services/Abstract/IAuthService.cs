using BookShopAppAPI.Services;
using BookShopAppAPI.Contracts;

namespace BookShopAppAPI.Services.Abstract;

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterRequest request);
    Task<LoginUserResponse?> AuthenticateAsync(LoginRequest request);
}
