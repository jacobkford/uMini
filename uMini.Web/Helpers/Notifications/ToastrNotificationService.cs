using Microsoft.Extensions.Options;

namespace uMini.Web.Helpers.Notifications;

public class ToastrNotificationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    private readonly ToastrNotificationOptions _defaultOptions;

    public ToastrNotificationService(
        IHttpContextAccessor httpContextAccessor, 
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IOptions<ToastrNotificationOptions> defaultOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _defaultOptions = defaultOptions?.Value ?? new ToastrNotificationOptions();
    }

    public string? RenderNotifications()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null)
            return null;

        var tempData = _tempDataDictionaryFactory.GetTempData(context);

        if (!tempData.ContainsKey("Notifications")) 
            return null;

        var notificationData = tempData["Notifications"]!.ToString();

        var jsBody = new StringBuilder();
        var notifications = JsonSerializer.Deserialize<List<ToastrNotification>>(notificationData!);

        jsBody.AppendLine("<script>");
        foreach (var note in notifications!)
        {
            jsBody.AppendLine($"toastr.{note.NotificationTypeString}('{note.Message}', '{note.Title}', {note.OptionsString});");
        }
        jsBody.AppendLine("</script>");

        Clear();
        return jsBody.ToString();
    }

    /// <summary>
    /// Clears session["Notifications"] object
    /// </summary>
    public void Clear()
    {
        var tempData = _tempDataDictionaryFactory.GetTempData(_httpContextAccessor.HttpContext);
        tempData.Remove("Notifications");
    }

    public void Add(ToastrNotification notification)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null)
            return;

        notification.Options ??= _defaultOptions;

        var tempData = _tempDataDictionaryFactory.GetTempData(context);

        var notifications = new List<ToastrNotification>();

        if (tempData.ContainsKey("Notifications"))
        {
            var notificationData = tempData["Notifications"]!.ToString();
            notifications = JsonSerializer.Deserialize<List<ToastrNotification>>(notificationData!);
        }
        notifications!.Add(notification);
        tempData["Notifications"] = JsonSerializer.Serialize(notifications);
    }
}

public static class ToastrNotificationExtensions
{
    public static IServiceCollection AddToastrNotifications(this IServiceCollection services)
    {
        services.AddTransient<ToastrNotificationService>();
        return services;
    }

    public static IServiceCollection AddToastrNotifications(this IServiceCollection services, Action<ToastrNotificationOptions> options)
    {
        services.AddTransient<ToastrNotificationService>();
        services.Configure(options);
        return services;
    }
}
