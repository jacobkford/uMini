using System.ComponentModel.DataAnnotations;

namespace uMini.Infrastructure.Entities;

public class ShortUrl
{
    public int Id { get; set; }

    /// <summary>
    /// The value that will be used as a param to redirect to the specified URL.
    /// </summary>
    public string Key { get; set; } = default!;

    /// <summary>
    /// The URL that will be redirected to when the specified key is passed as a param
    /// </summary>
    public string Url { get; set; } = default!;

    public string? CreatorId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
