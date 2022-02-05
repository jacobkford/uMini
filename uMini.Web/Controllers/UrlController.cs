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
    public async Task<IActionResult> Create(CreateShortUrlViewModel request)
    {
        request.CreatorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var keyAlreadyExists = await _shortUrlRepository.FindAsync(request.Key);

        if (keyAlreadyExists is not null)
        {
            ModelState.AddModelError("key", "A MiniUrl already exists with this name.");
        }

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

            TempData["success"] = "Successfully created MiniUrl!";

            return RedirectToAction("Index");
        }

        return View(request);
    }

    public async Task<IActionResult> Edit(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return NotFound();
        }
            
        var miniUrl = await _shortUrlRepository.FindAsync(key);

        if (miniUrl == null)
        {
            return NotFound();
        }

        var viewModel = new EditShortUrlViewModel { Key = miniUrl.Key, Url = miniUrl.Url };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditShortUrlViewModel request)
    {
        if (ModelState.IsValid)
        {
            var shortUrl = await _shortUrlRepository.FindAsync(request.Key);

            if (shortUrl == null)
            {
                return NotFound();
            }

            shortUrl.Url = request.Url;

            await _shortUrlRepository.Save();

            TempData["success"] = "Successfully editted MiniUrl!";

            return RedirectToAction("Index");
        }
        return View(request);  
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

        TempData["success"] = "Successfully deleted MiniUrl!";

        return RedirectToAction("Index");
    }
}
