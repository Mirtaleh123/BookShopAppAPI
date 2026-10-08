using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUserRepository users) : ControllerBase
{
    private readonly PasswordHasher<AppUser> _hasher = new();

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (await users.UsernameExistsAsync(request.Username))
            return Conflict(new { message = "Bu istifadəçi adı artıq mövcuddur." });
        if (await users.EmailExistsAsync(request.Email))
            return Conflict(new { message = "Bu email artıq qeydiyyatdan keçib." });

        var user = new AppUser
        {
            Username = request.Username,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        await users.AddAsync(user);
        await users.SaveAsync();
        return StatusCode(StatusCodes.Status201Created, new { user.Id, user.Username, user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await users.FindForLoginAsync(request.UsernameOrEmail);
        if (user is null || _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "İstifadəçi adı/email və ya şifrə yanlışdır." });

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return Ok(new { message = "Giriş uğurludur.", user = new { user.Id, user.Username, user.Role } });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Çıxış edildi." });
    }
}
