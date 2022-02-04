namespace uMini.Web.Models;

public class EditShortUrlViewModel
{
    /// <summary>
    /// The value that will be used as a param to redirect to the specified URL.
    /// </summary>
    public string Key { get; set; } = default!;

    /// <summary>
    /// The URL that will be redirected to when the specified key is passed as a param
    /// </summary>
    public string Url { get; set; } = default!;
}
