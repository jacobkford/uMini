namespace uMini.Infrastructure.Entities;

public class ApplicationUser : IdentityUser
{
    public string? LockoutReason { get; set; }
    public string? LockedOutBy { get; set; }
}
