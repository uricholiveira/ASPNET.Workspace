using System.Security.Claims;
using Business.Interfaces;
using Data.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest data, CancellationToken cancellationToken)
    {
        var result = await _authService.Login(data, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("login/refresh")]
    public async Task<IActionResult> LoginRefresh(CancellationToken cancellationToken)
    {
        var user = User.Identity as ClaimsIdentity;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest();

        var result = await _authService.Login(userId, cancellationToken);

        return Ok(result);
    }
}