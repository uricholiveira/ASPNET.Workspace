using System.Security.Claims;
using Business.Interfaces;
using Data.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest data, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUser(data, cancellationToken);

        return Ok(result);
    }

    [HttpGet("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromQuery] string userId,
        [FromQuery] string emailConfirmationToken,
        CancellationToken cancellationToken)
    {
        var result =
            await _userService.VerifyEmail(userId, emailConfirmationToken, cancellationToken);

        return result ? Ok("Email confirmado") : BadRequest("Email não confirmado");
    }

    [HttpGet("reset-password")]
    [Authorize]
    public async Task<IActionResult> ResetPassword(CancellationToken cancellationToken)
    {
        var user = User.Identity as ClaimsIdentity;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest();

        var result = await _userService.ResetPassword(userId, cancellationToken);

        return result is not null
            ? Ok(result)
            : BadRequest("Erro ao enviar o email de redefinição de senha");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] ResetPasswordRequest data,
        CancellationToken cancellationToken)
    {
        var user = User.Identity as ClaimsIdentity;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return BadRequest();

        var result = await _userService.UpdatePassword(userId, data, cancellationToken);

        return result!.Succeeded
            ? Ok()
            : BadRequest("Erro ao redefinir senha");
    }
}