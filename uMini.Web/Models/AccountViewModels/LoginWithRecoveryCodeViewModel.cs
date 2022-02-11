namespace uMini.Web.Models.AccountViewModels;

public class LoginWithRecoveryCodeViewModel
{
    [Required]
    public string Code { get; set; }

    public string ReturnUrl { get; set; }
}
