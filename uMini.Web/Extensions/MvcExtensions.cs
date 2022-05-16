namespace uMini.Web.Extensions;

public static class MvcExtensions
{
    public static string IsActive(this IHtmlHelper htmlHelper, string controllers = null, string actions = null, string cssClass = "active")
    {
        var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
        var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

        var acceptedControllers = (controllers ?? currentController ?? "").Split(',');
        var acceptedActions = (actions ?? currentAction ?? "").Split(',');

        return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction)
            ? cssClass
            : "";
    }

    public static string DisabledIf(this IHtmlHelper htmlHelper, bool condition)
    {
        return condition ? "disabled" : "";
    }

    public static HtmlString AddDateData(this IHtmlHelper htmlHelper, DateTimeOffset? time)
    {
        return new HtmlString(time.HasValue ? $"data-time=\"{time.Value:dd/MM/yyyy}\"" : "");
    }

    public static HtmlString AddTimeData(this IHtmlHelper htmlHelper, DateTimeOffset? time)
    {
        return new HtmlString(time.HasValue ? $"data-time=\"{time.Value:HH:mm}\"" : "");
    }
}
