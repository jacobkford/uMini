namespace uMini.Web.Areas.Identity.Models.Account;

public class LoginWithRecoveryCodeViewModel
{
    [Required]
    public string Code { get; set; }

    public string ReturnUrl { get; set; }
}
