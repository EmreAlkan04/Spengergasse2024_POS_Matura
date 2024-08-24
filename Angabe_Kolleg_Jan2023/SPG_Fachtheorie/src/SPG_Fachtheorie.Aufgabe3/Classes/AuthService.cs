using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe3.Classes
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> TryLogin(string registrationNumber)
        {
            if (_httpContextAccessor.HttpContext is null) { return false; }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, registrationNumber),
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

        public int AdminId => Int32.Parse(_httpContextAccessor.HttpContext?.User.Identity?.Name ?? "0");
    }
}