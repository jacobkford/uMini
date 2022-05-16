namespace uMini.Web.Areas.User.Models.Urls;

public class UserUrlViewModel
{
    [Display(Name = "No.")]
    public string Key { get; set; } = default!;

    [Display(Name = "LongUrl")]
    public string Url { get; set; } = default!;

    [Display(Name = "MiniUrl")]
    public string CustomUrl { get; init; } = default!;

    [Display(Name = "Owner Id")]
    public string? CreatorId { get; set; }

    [Display(Name = "Created")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
    public DateTime CreatedDate { get; set; }
}
