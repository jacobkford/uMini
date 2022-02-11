﻿namespace uMini.Web.Models.AccountViewModels;

public class ExternalLoginConfirmationViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
