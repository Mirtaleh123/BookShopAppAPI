using BookShopAppAPI.Contracts;
using BookShopAppAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await auth.RegisterAsync(request);
        return result.Outcome switch
        {
            RegisterOutcome.UsernameExists => Conflict(new { message = "Bu istifadəçi adı artıq mövcuddur." }),
            RegisterOutcome.EmailExists => Conflict(new { message = "Bu email artıq qeydiyyatdan keçib." }),
            _ => StatusCode(StatusCodes.Status201Created, result.User)
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await auth.AuthenticateAsync(request);
        if (user is null)
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
        return Ok(new { message = "Giriş uğurludur.", user });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Çıxış edildi." });
    }
}
