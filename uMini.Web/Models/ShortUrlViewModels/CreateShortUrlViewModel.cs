namespace uMini.Web.Models.ShortUrlViewModels;

public class CreateShortUrlViewModel
{
    /// <summary>
    /// The value that will be used as a param to redirect to the specified URL.
    /// </summary>
    [Display(Name = "Custom Url Name")]
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(20, ErrorMessage = "Name can not be longer than 20 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_-]*$", ErrorMessage = "Name can only use uppercase and lowercase letters, numbers, underscores, and hyphens/dashes")]
    public string Key { get; set; } = default!;

    /// <summary>
    /// The URL that will be redirected to when the specified key is passed as a param
    /// </summary>
    [Display(Name = "Long Url")]
    [Required(ErrorMessage = "Url is required")]
    [MaxLength(1000, ErrorMessage = "Url can not be longer than 1000 characters")]
    public string Url { get; set; } = default!;

    public string? CreatorId { get; set; }
}
