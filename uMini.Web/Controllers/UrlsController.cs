﻿namespace uMini.Web.Controllers;

[Authorize]
public class UrlsController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IShortUrlRepository _shortUrlRepository;

    public UrlsController(ILogger<HomeController> logger, IShortUrlRepository shortUrlRepository)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<IActionResult> Manage()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);

        var urls = await _shortUrlRepository.FindAllByUserIdAsync(userId?.Value);

        return View(urls);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateShortUrlViewModel request)
    {
        var newShortUrl = new ShortUrl
        {
            Key = request.Key,
            Url = request.Url,
            CreatorId = request.CreatorId,
        };

        await _shortUrlRepository.Add(newShortUrl);
        await _shortUrlRepository.Save();

        return RedirectToAction("Manage");
    }
}
