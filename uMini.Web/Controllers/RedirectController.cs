namespace uMini.Web.Controllers;

[AllowAnonymous]
public class RedirectController : Controller
{
    private readonly ILogger<RedirectController> _logger;
    private readonly IShortUrlRepository _shortUrlRepository;

    public RedirectController(ILogger<RedirectController> logger, IShortUrlRepository shortUrlRepository)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
    }

    [HttpGet("/{key}")]
    public async Task<IActionResult> RedirectTo(string key)
    {
        var data = await _shortUrlRepository.FindAsync(key);

        if (data is null)
        {
            TempData["error"] = $"Could not find a ShortUrl with the name {key}";
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
        }

        return Redirect(data.Url);
    }
}
