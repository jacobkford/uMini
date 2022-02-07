namespace uMini.Web.Controllers;

[Authorize]
[Route("my/urls/{action=Index}")]
public class UrlController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IShortUrlRepository _shortUrlRepository;
    private readonly IMapper _mapper;

    public UrlController(ILogger<HomeController> logger, IShortUrlRepository shortUrlRepository, IMapper mapper)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);

        var urls = await _shortUrlRepository.FindAllByUserIdAsync(userId?.Value);

        var data = _mapper.Map<IEnumerable<ShortUrl>, IEnumerable<ShortUrlViewModel>>(urls);

        return View(data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Key,Url")] CreateShortUrlViewModel request)
    {
        var keyAlreadyExists = await _shortUrlRepository.FindAsync(request.Key);

        if (keyAlreadyExists is not null)
        {
            ModelState.AddModelError("key", "A MiniUrl already exists with this name.");
        }

        if (!request.Url.IsValidUrl())
        {
            ModelState.AddModelError("url", "Invalid Url, make sure it starts with either http:// or https://");
        }

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            var newShortUrl = new ShortUrl
            {
                Key = request.Key,
                Url = request.Url,
                CreatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            };

            await _shortUrlRepository.Add(newShortUrl);
            await _shortUrlRepository.Save();

            TempData["success"] = "Successfully created MiniUrl!";
        }
        catch (Exception ex)
        {
            TempData["error"] = $"Unable to save changes. Try again, and if the problem persists see your system administrator.";
            Console.WriteLine(ex);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string key)
    {           
        var miniUrl = await _shortUrlRepository.FindAsync(key);

        if (miniUrl is null)
        {
            TempData["error"] = "Internal error, couldn't find entry to edit";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new EditShortUrlViewModel { Key = miniUrl.Key, Url = miniUrl.Url };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditShortUrlViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);  
        }

        var shortUrl = await _shortUrlRepository.FindAsync(request.Key);
        if (shortUrl is not null)
        {
            shortUrl.Url = request.Url;
            await _shortUrlRepository.Save();
            TempData["success"] = "Successfully editted MiniUrl!";
        }
        else
        {
            TempData["error"] = "Internal error, couldn't find entry to edit";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string key)
    {
        var shortUrl = await _shortUrlRepository.FindAsync(key);
        if (shortUrl is not null)
        {
            _shortUrlRepository.Delete(shortUrl);
            await _shortUrlRepository.Save();
            TempData["success"] = "Successfully deleted MiniUrl!";
        }
        else
        {
            TempData["error"] = "Internal error, couldn't find entry to delete";
        }

        return RedirectToAction(nameof(Index));
    }
}
