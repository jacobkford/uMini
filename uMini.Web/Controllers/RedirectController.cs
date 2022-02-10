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
            return RedirectToAction(nameof(ShortUrlNotFound));
        }

        return Redirect(data.Url);
    }

    public IActionResult ShortUrlNotFound(string key)
    {
        return View(key);
    }
}
