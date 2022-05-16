namespace uMini.Web.Helpers.Notifications;

public class ToastrNotificationOptions
{
    public bool TapToDismiss { get; set; } = true;

    public string ToastClass { get; set; } = "toast";

    public string ContainerId { get; set; } = "toast-container";

    public bool Debug { get; set; } = false;

    public string ShowMethod { get; set; } = "fadeIn";

    public int ShowDuration { get; set; } = 300;

    public string ShowEasing { get; set; } = "swing";

    public string HideMethod { get; set; } = "fadeOut";

    public int HideDuration { get; set; } = 1000;

    public string HideEasing { get; set; } = "swing";

    public bool CloseMethod { get; set; } = false;

    public bool CloseDuration { get; set; } = false;

    public bool CloseEasing { get; set; } = false;

    public bool CloseOnHover { get; set; } = true;

    public int ExtendedTimeOut { get; set; } = 1000;

    public IDictionary<string, string> IconClasses { get; set; } = new Dictionary<string, string>() 
    {
        { "error", "toast-error" },
        { "info", "toast-info" },
        { "success", "toast-success" },
        { "warning", "toast-warning" }
    };

    public string PositionClass { get; set; } = "toast-top-right";

    public int TimeOut { get; set; } = 5000;

    public string TitleClass { get; set; } = "toast-title";

    public string MessageClass { get; set; } = "toast-message";

    public bool EscapeHtml { get; set; } = false;

    public string Target { get; set; } = "body";

    public string CloseClass { get; set; } = "toast-close-button";

    public bool NewestOnTop { get; set; } = true;

    public bool PreventDuplicates { get; set; } = false;

    public bool ProgressBar { get; set; } = false;

    public string ProgressClass { get; set; } = "toast-progress";

    public bool Rtl { get; set; } = false;
}
