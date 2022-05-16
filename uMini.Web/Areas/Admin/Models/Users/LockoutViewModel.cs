namespace uMini.Web.Areas.Admin.Models.Users;

public class LockoutViewModel
{
    public string UserId { get; set; }

    [Display(Name = "Expiration Date")]
    [Required(ErrorMessage = "Expiration Date is required")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTimeOffset? EndDate { get; set; }

    [Display(Name = "Reason")]
    [Required(ErrorMessage = "Reason is required")]
    public string Reason { get; set; }

    public string AuthorisedById { get; set; }
}
