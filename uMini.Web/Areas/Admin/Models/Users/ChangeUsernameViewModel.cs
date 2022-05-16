namespace uMini.Web.Areas.Admin.Models.Users;

public class ChangeUsernameViewModel
{
    public string UserId { get; set; }

    public string OldUsername { get; set; }

    [Display(Name = "New Username")]
    [Required(ErrorMessage = "A new Username is required")]
    public string NewUsername { get; set; }
}
