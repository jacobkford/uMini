namespace uMini.Web.Controllers;

[Authorize]
[Route("my/urls/{action=Index}")]
public class UserUrlsController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IShortUrlRepository _shortUrlRepository;

    public UserUrlsController(ILogger<HomeController> logger, IShortUrlRepository shortUrlRepository)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);

        var urls = await _shortUrlRepository.FindAllByUserIdAsync(userId?.Value);

        return View(urls);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateShortUrlViewModel request)
    {
        request.CreatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (ModelState.IsValid)
        {
            var newShortUrl = new ShortUrl
            {
                Key = request.Key,
                Url = request.Url,
                CreatorId = request.CreatorId,
            };

            await _shortUrlRepository.Add(newShortUrl);
            await _shortUrlRepository.Save();

            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditShortUrlViewModel request)
    {
        var shortUrl = await _shortUrlRepository.FindAsync(request.Key);

        if (shortUrl == null)
        {
            return NotFound();
        }

        shortUrl.Url = request.Url;

        await _shortUrlRepository.Save();

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string key)
    {
        var shortUrl = await _shortUrlRepository.FindAsync(key);

        if (shortUrl == null)
        {
            return NotFound();
        }

        _shortUrlRepository.Delete(shortUrl);
        await _shortUrlRepository.Save();

        return RedirectToAction("Index");
    }
}
