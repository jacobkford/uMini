namespace uMini.Web.Helpers;

public class PageSizeSelectList : List<SelectListItem>
{
    public PageSizeSelectList()
    {
        this.AddRange(new SelectListItem[]
        {
            new SelectListItem() { Value = "5", Text="5" },
            new SelectListItem() { Value = "10", Text="10" },
            new SelectListItem() { Value = "15", Text="15" },
            new SelectListItem() { Value = "25", Text="25" },
            new SelectListItem() { Value = "50", Text="50" },
        });
    }
}
