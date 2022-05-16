﻿namespace uMini.Web.Areas.Admin.Models.Users;

public class ChangeEmailViewModel
{
    public string UserId { get; set; }

    public string OldEmail { get; set; }

    [EmailAddress]
    [Display(Name = "New Username")]
    [Required(ErrorMessage = "A new Username is required")]
    public string NewEmail { get; set; }
}
