namespace uMini.Web.Areas.Admin.Models.Users;

public class ManageRolesViewModel
{
    public string UserId { get; set; }
    public ClaimsPrincipal ContextUser { get; set; }
    public IList<string> UserRoles { get; set; }
    public List<SelectListItem> AvailableRoles { get; set; }
}
