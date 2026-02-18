using FinanceService.Application.Contract.IService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace FinanceService.Infrastructure.Contract.Service;

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _http;

    public UserContext(IHttpContextAccessor http) => _http = http;

    public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var user = _http.HttpContext?.User;
            if (user is null) return Guid.Empty;

            // Adjust claim type to your token: "sub", "uid", ClaimTypes.NameIdentifier, etc.
            string? id = user.FindFirstValue("sub") ?? user.FindFirstValue("uid") ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }
    }
}