namespace uMini.Web.Areas.My.Controllers;

[Authorize]
[Area("User")]
public class UrlsController : Controller
{
    private readonly ILogger<UrlsController> _logger;
    private readonly IShortUrlRepository _shortUrlRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ToastrNotificationService _notificationHandler;

    public UrlsController(
        ILogger<UrlsController> logger,
        IShortUrlRepository shortUrlRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        ToastrNotificationService notificationHandler)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _notificationHandler = notificationHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string sort, string filter, string query, int? page, int? size)
    {
        ViewData["CurrentSort"] = sort;
        ViewData["ShortUrlSortParm"] = string.IsNullOrEmpty(sort) ? "shortUrl_desc" : "";
        ViewData["LongUrlSortParm"] = sort == "longUrl" ? "longUrl_desc" : "longUrl";
        ViewData["DateSortParm"] = sort == "date" ? "date_desc" : "date";

        int pageSize = 10;
        int pageMaxSize = 50;

        if (size is not null)
            pageSize = size > pageMaxSize ? pageMaxSize : (int)size;

        var pageSizeList = new PageSizeSelectList();

        foreach (var item in pageSizeList.Where(x => x.Value == pageSize.ToString()))
            item.Selected = true;

        ViewData["PageSize"] = pageSize;
        ViewData["PageSizeList"] = pageSizeList;

        if (query != null)
            page = 1;
        else
            query = filter;

        ViewData["CurrentFilter"] = query;

        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        var urls = await _shortUrlRepository.FindAllByUserIdAsync(userId?.Value);
        var data = _mapper.Map<IEnumerable<ShortUrl>, IEnumerable<UserUrlViewModel>>(urls);

        if (!string.IsNullOrEmpty(query))
        {
            data = data.Where(u => u.Key.Contains(query) || u.Url.Contains(query));
        }

        data = sort switch
        {
            "shortUrl_desc" => data.OrderByDescending(s => s.Key),
            "longUrl" => data.OrderBy(s => s.Url),
            "longUrl_desc" => data.OrderByDescending(s => s.Url),
            "date" => data.OrderBy(s => s.CreatedDate),
            "date_desc" => data.OrderByDescending(s => s.CreatedDate),
            _ => data.OrderBy(s => s.Key),
        };

        return View(PaginatedList<UserUrlViewModel>.Create(data, page ?? 1, pageSize));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Key,Url")] CreateUserUrlViewModel request)
    {
        var keyAlreadyExists = await _shortUrlRepository.FindAsync(request.Key);

        if (keyAlreadyExists is not null)
            ModelState.AddModelError("key", "A MiniUrl already exists with this name.");

        if (!request.Url.IsValidUrl())
            ModelState.AddModelError("url", "Invalid Url, make sure it starts with either http:// or https://");

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(x => x.Errors))
                _notificationHandler.Add(new ToastrNotification(error.ErrorMessage, ToastrNotificationType.Error));

            return View(request);
        }

        try
        {
            var newShortUrl = new ShortUrl
            {
                Key = request.Key,
                Url = request.Url,
                CreatorId = _userManager.GetUserId(User),
            };

            await _shortUrlRepository.Add(newShortUrl);
            await _shortUrlRepository.Save();

            _notificationHandler.Add(new ToastrNotification("Successfully created a MiniUrl!", ToastrNotificationType.Success));
        }
        catch (Exception ex)
        {
            _notificationHandler.Add(new ToastrNotification(
                "Internal error, unable to save changes", "Try again, and if the problem persists see your system administrator",
                ToastrNotificationType.Error));

            _logger.Log(LogLevel.Error, ex.Message, ex.StackTrace);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string key)
    {
        var miniUrl = await _shortUrlRepository.FindAsync(key);

        if (miniUrl is null || miniUrl.CreatorId != _userManager.GetUserId(User))
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t find entry to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new EditUserUrlViewModel { Key = miniUrl.Key, Url = miniUrl.Url };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserUrlViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var miniUrl = await _shortUrlRepository.FindAsync(request.Key);
        if (miniUrl is not null || miniUrl!.CreatorId == _userManager.GetUserId(User))
        {
            miniUrl.Url = request.Url;
            await _shortUrlRepository.Save();
            _notificationHandler.Add(new ToastrNotification("Successfully editted MiniUrl!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t find entry to edit", ToastrNotificationType.Error));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string key)
    {
        var miniUrl = await _shortUrlRepository.FindAsync(key);
        if (miniUrl is not null || miniUrl!.CreatorId == _userManager.GetUserId(User))
        {
            _shortUrlRepository.Delete(miniUrl);
            await _shortUrlRepository.Save();
            _notificationHandler.Add(new ToastrNotification("Successfully deleted MiniUrl!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t find entry to delete", ToastrNotificationType.Error));
        }

        return RedirectToAction(nameof(Index));
    }
}
