using System.Security.Claims;
using Data.Models;
using Data.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces;

public interface IAuthService
{
    public Task<JwtToken?> GenerateToken(IdentityUser user);
    public Task<JwtToken?> Login(string userId, CancellationToken cancellationToken);
    public Task<JwtToken?> Login(LoginRequest data, CancellationToken cancellationToken);
}