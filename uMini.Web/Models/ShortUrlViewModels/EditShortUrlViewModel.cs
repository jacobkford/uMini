namespace uMini.Web.Models.ShortUrlViewModels;

public class EditShortUrlViewModel
{
    /// <summary>
    /// The value that will be used as a param to redirect to the specified URL.
    /// </summary>
    public string Key { get; set; } = default!;

    /// <summary>
    /// The URL that will be redirected to when the specified key is passed as a param
    /// </summary>
    [Display(Name = "Long Url")]
    [Required(ErrorMessage = "Url is required")]
    [MaxLength(1000, ErrorMessage = "Url can not be longer than 1000 characters")]
    public string Url { get; set; } = default!;
}
