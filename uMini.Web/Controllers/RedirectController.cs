namespace uMini.Web.Controllers;

public class RedirectController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IShortUrlRepository _shortUrlRepository;

    public RedirectController(ILogger<HomeController> logger, IShortUrlRepository shortUrlRepository)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
    }

    [HttpGet("/{key}")]
    public async Task<IActionResult> RedirectTo(string key)
    {
        var data = await _shortUrlRepository.FindAsync(key);

        if (data != null)
        {
            return Redirect(data.Url);
        }

        return View("ShortUrlNotFound", key);
    }
}
