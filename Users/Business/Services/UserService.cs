using Business.Interfaces;
using Data.Models;
using Data.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class UserService : IUserService
{
    private readonly JwtOptions _jwtOptions;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(JwtOptions jwtOptions, SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
    {
        _jwtOptions = jwtOptions;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUser(CreateUserRequest data, CancellationToken cancellationToken)
    {
        var user = new IdentityUser
        {
            UserName = data.Email,
            Email = data.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, data.Password);
        return result;
    }

    public async Task<IdentityResult?> UpdatePassword(string userId, ResetPasswordRequest data,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return null;

        var result = await _userManager.ResetPasswordAsync(user, data.ResetPasswordToken, data.Password);
        return result;
    }

    public async Task<string?> ResetPassword(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return null;

        var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        return resetPasswordToken;
    }

    public async Task<bool> VerifyEmail(string userId, string emailConfirmationToken,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
        return result.Succeeded;
    }
}