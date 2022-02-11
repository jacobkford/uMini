namespace uMini.Web.Models.ManageViewModels;
public class DisplayRecoveryCodesViewModel
{
    [Required]
    public IEnumerable<string> Codes { get; set; }

}
