namespace uMini.Web.Models;

public class UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public string? LockoutReason { get; set; }
    public string? LockoutAuthorisedById { get; set; }
    public bool IsLockedOut { get => LockoutEnabled && LockoutEnd is not null && LockoutEnd >= DateTime.Now; }
    public IEnumerable<string> Roles { get; set; }
}
