namespace uMini.Web.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IShortUrlRepository _shortUrlRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ToastrNotificationService _notificationHandler;

    public HomeController(
        ILogger<HomeController> logger, 
        IHttpContextAccessor httpContextAccessor,
        IShortUrlRepository shortUrlRepository,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ToastrNotificationService notificationHandler)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _shortUrlRepository = shortUrlRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _notificationHandler = notificationHandler;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CreateUrlViewModel request)
    {
        var keyAlreadyExists = await _shortUrlRepository.FindAsync(request.Key);

        if (keyAlreadyExists is not null)
            ModelState.AddModelError(nameof(request.Key), "A MiniUrl already exists with this name");

        if (!request.Url.IsValidUrl())
            ModelState.AddModelError(nameof(request.Url), "Invalid Url, make sure it starts with either http:// or https://");

        var errorNotificationOptions = new ToastrNotificationOptions()
        {
            PositionClass = "toast-bottom-right",
            ShowDuration = 10000
        };

        if (!ModelState.IsValid)
        { 
            foreach (var error in ModelState.Values.SelectMany(x => x.Errors))
                _notificationHandler.Add(new ToastrNotification(
                    error.ErrorMessage, ToastrNotificationType.Error, errorNotificationOptions));
            
            return View(request);
        }
            
        try
        {
            var newShortUrl = new ShortUrl { Key = request.Key, Url = request.Url };

            if (_signInManager.IsSignedIn(User))
                newShortUrl.CreatorId = _userManager.GetUserId(User);

            await _shortUrlRepository.Add(newShortUrl);
            await _shortUrlRepository.Save();

            _notificationHandler.Add(new ToastrNotification("Successfully created a MiniUrl!", ToastrNotificationType.Success));
        }
        catch (Exception ex)
        {
            _notificationHandler.Add(new ToastrNotification(
                "Internal error, unable to save changes", "Try again, and if the problem persists see your system administrator",
                ToastrNotificationType.Error, errorNotificationOptions));

            _logger.Log(LogLevel.Error, ex.Message, ex.StackTrace);
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
