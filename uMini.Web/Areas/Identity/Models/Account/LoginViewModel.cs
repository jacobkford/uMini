namespace uMini.Web.Areas.Identity.Models.Account;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Email / Username")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
