using System.Security.Claims;

namespace uMini.Infrastructure.Extensions;

public static class UserManagerExtensions
{
    public static async Task<IdentityResult> LockoutAsync(this UserManager<ApplicationUser> manager, string userToBanId, string authorisedBy, DateTimeOffset? endDate, string? reason)
    {
        var user = await manager.FindByIdAsync(userToBanId);
        if (user == null)
            return IdentityResult.Failed(manager.ErrorDescriber.InvalidToken());

        var setLockoutTimeResult = await manager.SetLockoutEndDateAsync(user, endDate);
        if (setLockoutTimeResult.Errors.Any())
            return IdentityResult.Failed(setLockoutTimeResult.Errors.ToArray());

        user.LockedOutBy = authorisedBy;
        user.LockoutReason = reason;

        var updateResult = await manager.UpdateAsync(user);
        if (updateResult.Errors.Any())
            return IdentityResult.Failed(updateResult.Errors.ToArray());

        return IdentityResult.Success;
    }

    public static async Task<IdentityResult> DisableCurrentLockoutAsync(this UserManager<ApplicationUser> manager, string userToUnbanId)
    {
        var user = await manager.FindByIdAsync(userToUnbanId);
        if (user == null)
            return IdentityResult.Failed(manager.ErrorDescriber.InvalidToken());

        user.LockoutEnd = null;
        user.LockedOutBy = null;
        user.LockoutReason = null;

        var updateResult = await manager.UpdateAsync(user);
        if (updateResult.Errors.Any())
            return IdentityResult.Failed(updateResult.Errors.ToArray());

        return IdentityResult.Success;
    }

    public static async Task<bool> IsLockedOutAsync(this UserManager<ApplicationUser> manager, ClaimsPrincipal principal)
    {
        var userId = manager.GetUserId(principal);
        var user = await manager.FindByIdAsync(userId);

        return user.LockoutEnabled
            && user.LockoutEnd.HasValue
            && user.LockoutEnd.Value.ToUniversalTime() <= DateTimeOffset.UtcNow;
    }

    public static bool IsLockedOut(this UserManager<ApplicationUser> manager, ApplicationUser user)
    {
        return user.LockoutEnabled
            && user.LockoutEnd.HasValue
            && user.LockoutEnd.Value.ToUniversalTime() <= DateTimeOffset.UtcNow;
    }
}
