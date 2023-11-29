using Data.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces;

public interface IUserService
{
    public Task<IdentityResult> CreateUser(CreateUserRequest data, CancellationToken cancellationToken);

    public Task<IdentityResult?> UpdatePassword(string userId, ResetPasswordRequest data,
        CancellationToken cancellationToken);

    public Task<string?> ResetPassword(string userId, CancellationToken cancellationToken);

    public Task<bool> VerifyEmail(string userId, string emailConfirmationToken,
        CancellationToken cancellationToken);
}