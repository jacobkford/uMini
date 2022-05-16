namespace uMini.Web.ViewComponents;

public class ApplicationUserRoleChips : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserRoleChips(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(string userId, ClaimsPrincipal contextUser, string chipSizeClass = "", bool removeRoleModalButton = false)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var userRoles = await _userManager.GetRolesAsync(user);

        return View(new ApplicationUserRoleChipsViewModel
        {
            Roles = userRoles,
            ContextUser = contextUser,
            ChipSizeClass = chipSizeClass,
            RemoveRoleModalButton = removeRoleModalButton,
        });
    }

    public static string ChipColor(string role)
    {
        return role switch
        {
            ApplicationUserRole.SuperAdmin => "btn-outline-warning",
            ApplicationUserRole.Admin => "btn-outline-danger",
            ApplicationUserRole.Moderator => "btn-outline-info",
            ApplicationUserRole.Member => "btn-outline-dark",
            _ => ""
        };
    }
}
