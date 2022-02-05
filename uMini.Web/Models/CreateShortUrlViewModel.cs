﻿namespace uMini.Web.Models;

public class CreateShortUrlViewModel
{
    /// <summary>
    /// The value that will be used as a param to redirect to the specified URL.
    /// </summary>
    [Required(ErrorMessage = "Key is Required")]
    [MaxLength(20)]
    [RegularExpression(@"^[a-zA-Z0-9_-]*$")]
    public string Key { get; set; } = default!;

    /// <summary>
    /// The URL that will be redirected to when the specified key is passed as a param
    /// </summary>
    [Required(ErrorMessage = "Url is Required")]
    [MaxLength(1000)]
    public string Url { get; set; } = default!;

    public string? CreatorId { get; set; }
}
