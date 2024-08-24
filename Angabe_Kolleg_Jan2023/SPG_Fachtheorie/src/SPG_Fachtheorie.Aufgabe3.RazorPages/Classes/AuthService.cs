using Microsoft.AspNetCore.Authentication;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using System.Security.Claims;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Classes
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PodcastContext _db;
        public AuthService(IHttpContextAccessor httpContextAccessor, PodcastContext db)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }
        public async Task<bool> TryLogin(int id)
        {
            if (_httpContextAccessor.HttpContext is null) { return false; }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, id.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
                //IsPersistent = true
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return true;
        }

        public Task Logout()
        {
            if (_httpContextAccessor.HttpContext is null) { return Task.CompletedTask; }
            return _httpContextAccessor.HttpContext.SignOutAsync();
        }

        public int? AdminId => int.TryParse(_httpContextAccessor.HttpContext?.User.Identity?.Name, out var adminId)
            ? adminId : null;
   }
}
