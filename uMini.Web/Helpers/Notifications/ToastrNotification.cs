namespace uMini.Web.Helpers.Notifications;

public class ToastrNotification
{
    public string Title { get; set; }
    public string Message { get; set; }
    public ToastrNotificationType NotificationType { get; set; }
    public string NotificationTypeString { get => NotificationType.ToString().ToLower(); }
    public ToastrNotificationOptions? Options { get; set; }
    public string OptionsString { get => JsonSerializer.Serialize(Options,
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        }); 
    }

    public ToastrNotification(string title, ToastrNotificationType notificationType)
    {
        Title = title;
        NotificationType = notificationType;
        Message = string.Empty;
    }

    public ToastrNotification(string title, ToastrNotificationType notificationType, ToastrNotificationOptions options)
    {
        Title = title;
        NotificationType = notificationType;
        Message = string.Empty;
        Options = options;
    }

    public ToastrNotification(string title, string message, ToastrNotificationType notificationType)
    {
        Title = title;
        NotificationType = notificationType;
        Message = message;
    }

    [JsonConstructor]
    public ToastrNotification(string title, string message, ToastrNotificationType notificationType, ToastrNotificationOptions options)
    {
        Title = title;
        NotificationType = notificationType;
        Message = message;
        Options = options;
    }
}
