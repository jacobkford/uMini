namespace uMini.Web.ViewComponents;

public class AdminDashboardUserCardHeader : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminDashboardUserCardHeader(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var userRoles = await _userManager.GetRolesAsync(user);

        return View(new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            LockoutReason = user.LockoutReason,
            LockoutAuthorisedById = user.LockedOutBy,
            Roles = userRoles,
        });
    }
}
