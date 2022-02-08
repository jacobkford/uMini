namespace uMini.Web.Models;

public class ShortUrlViewModel
{
    [Display(Name = "Custom Url Key")]
    public string Key { get; set; } = default!;

    [Display(Name = "LongUrl")]
    public string Url { get; set; } = default!;

    [Display(Name = "MiniUrl")]
    public string CustomUrl { get; init; } = default!;

    [Display(Name = "Creator User Id")]
    public string? CreatorId { get; set; }

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }
}
