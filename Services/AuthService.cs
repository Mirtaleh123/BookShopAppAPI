using AutoMapper;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BookShopAppAPI.Services;

public sealed class AuthService(
    IUserRepository users,
    IMapper mapper,
    IPasswordHasher<AppUser> hasher)
    : BaseService<IUserRepository>(users), IAuthService
{
    public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
    {
        if (await Repository.UsernameExistsAsync(request.Username))
            return new(RegisterOutcome.UsernameExists);
        if (await Repository.EmailExistsAsync(request.Email))
            return new(RegisterOutcome.EmailExists);

        var user = mapper.Map<AppUser>(request);
        user.PasswordHash = hasher.HashPassword(user, request.Password);
        await Repository.AddAsync(user);
        await SaveAsync();
        return new(RegisterOutcome.Created, mapper.Map<RegisteredUserResponse>(user));
    }

    public async Task<LoginUserResponse?> AuthenticateAsync(LoginRequest request)
    {
        var user = await Repository.FindForLoginAsync(request.UsernameOrEmail);
        if (user is null || hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
            return null;

        return mapper.Map<LoginUserResponse>(user);
    }
}
