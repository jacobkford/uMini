namespace uMini.Web.Models;

public class ApplicationUserRoleChipsViewModel
{
    public IEnumerable<string> Roles { get; set; }
    public ClaimsPrincipal ContextUser { get; set; }
    public string ChipSizeClass { get; set; }
    public bool RemoveRoleModalButton { get; set; }

}
