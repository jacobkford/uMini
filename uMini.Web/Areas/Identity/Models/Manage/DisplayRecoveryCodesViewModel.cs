namespace uMini.Web.Areas.Identity.Models.Manage;

public class DisplayRecoveryCodesViewModel
{
    [Required]
    public IEnumerable<string> Codes { get; set; }

}
