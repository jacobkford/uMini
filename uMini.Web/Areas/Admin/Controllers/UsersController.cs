using uMini.Infrastructure.Extensions;
using uMini.Web.Areas.Admin.Models;
using uMini.Web.Areas.Admin.Models.Users;

namespace uMini.Web.Areas.Admin.Controllers;

[Authorize(Roles = "SuperAdmin, Admin")]
[Area("Admin")]
public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ToastrNotificationService _notificationHandler;

    public UsersController(
        ILogger<UsersController> logger,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ToastrNotificationService notificationHandler)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _notificationHandler = notificationHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string sort, string filter, string query, int? page, int? size)
    {
        ViewData["CurrentSort"] = sort;
        ViewData["NameSortParm"] = string.IsNullOrEmpty(sort) ? "name_desc" : "";
        ViewData["EmailSortParm"] = sort == "email" ? "email_desc" : "email";

        int pageSize = 10;
        int pageMaxSize = 50;

        if (size is not null)
            pageSize = size > pageMaxSize ? pageMaxSize : (int)size;
        

        var pageSizeList = new PageSizeSelectList();

        foreach (var item in pageSizeList.Where(x => x.Value == pageSize.ToString()))
        {
            item.Selected = true;
        }

        ViewData["PageSize"] = pageSize;
        ViewData["PageSizeList"] = pageSizeList;

        if (query != null)
            page = 1;
        else
            query = filter;

        ViewData["CurrentFilter"] = query;

        var users = _userManager.Users.AsEnumerable();
        var userData = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            userData.Add(new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                LockoutReason = user.LockoutReason,
                LockoutAuthorisedById = user.LockedOutBy,
                Roles = roles
            });
        }

        var data = userData.AsEnumerable();

        data = sort switch
        {
            "name_desc" => data.OrderByDescending(s => s.UserName),
            "email" => data.OrderBy(s => s.Email),
            "email_desc" => data.OrderByDescending(s => s.Email),
            _ => data.OrderBy(s => s.UserName),
        };

        return View( PaginatedList<UserViewModel>.Create(data, page ?? 1, pageSize));
    }

    [HttpGet]
    public async Task<IActionResult> ChangeUsername(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        return View(new ChangeUsernameViewModel { UserId = user.Id, OldUsername = user.UserName });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUsername(ChangeUsernameViewModel request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        if (request.OldUsername == request.NewUsername)
        {
            ModelState.AddModelError(nameof(ChangeUsernameViewModel.NewUsername), "Please enter a username that is different to their current one");
            return View(request);
        }

        if (await _userManager.FindByNameAsync(request.NewUsername) is not null)
        {
            ModelState.AddModelError(nameof(ChangeUsernameViewModel.NewUsername), "There is already a user registered with this username");
            return View(request);
        }

        var result = await _userManager.SetUserNameAsync(user, request.NewUsername);
        if (result.Succeeded)
        {
            _notificationHandler.Add(new ToastrNotification("Successfully changed user\\'s username!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t change user\\'s username", ToastrNotificationType.Error));
            _logger.LogError("Error occured when changing username", result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ChangeEmail(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        return View(new ChangeEmailViewModel { UserId = user.Id, OldEmail = user.Email });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        if (request.OldEmail == request.NewEmail)
        {
            ModelState.AddModelError("username", "Please enter a username that is different to their current one");
            return View(request);
        }

        if (await _userManager.FindByNameAsync(request.NewEmail) is not null)
        {
            ModelState.AddModelError("email", "There\\'s already a user registered with this email");
            return View(request);
        }

        var result = await _userManager.SetEmailAsync(user, request.NewEmail);
        if (result.Succeeded)
        {
            _notificationHandler.Add(new ToastrNotification("Successfully changed user\\'s email!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t change user\\'s email", ToastrNotificationType.Error));
            _logger.LogError("Error occured when changing email", result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ManageRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var availableRoles = await _roleManager.Roles
            .Where(role => role.Name != ApplicationUserRole.SuperAdmin)
            .Select(role => new SelectListItem
            {
                Text = role.Name,
                Value = role.Name,
                Disabled = ShouldDisableRoleSelection(userRoles, role, User)
            })
            .ToListAsync();

        availableRoles.Insert(0, new SelectListItem("Select a role to add...", ""));

        return base.View(new ManageRolesViewModel
        {
            UserId = user.Id,
            UserRoles = userRoles,
            AvailableRoles = availableRoles
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRole(AddRoleViewModel request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (result.Succeeded)
        {
            _notificationHandler.Add(new ToastrNotification("Successfully added role to user!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t add role to user", ToastrNotificationType.Error));
            _logger.LogError("Error occured when adding role to user", result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRole(RemoveRoleViewModel request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to edit", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
        if (result.Succeeded)
        {
            _notificationHandler.Add(new ToastrNotification("Successfully removed role from user!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t remove role from user", ToastrNotificationType.Error));
            _logger.LogError("Error occured when removing role from user", result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Lockout(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        if (await _userManager.IsInRoleAsync(user, ApplicationUserRole.SuperAdmin))
        {
            _notificationHandler.Add(new ToastrNotification("Locking out SuperAdmin isn\\'t permitted", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        DateTimeOffset? lockoutEndDateTime = null;

        if (user.LockoutEnd is not null)
            lockoutEndDateTime = ToClientTime(user.LockoutEnd.Value);

        return View(new LockoutViewModel 
        { 
            UserId = user.Id,
            EndDate = lockoutEndDateTime,
            Reason = user.LockoutReason ?? "",
            AuthorisedById = user.LockedOutBy ?? ""
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Lockout(LockoutViewModel request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        if (await _userManager.IsInRoleAsync(user, ApplicationUserRole.SuperAdmin))
        {
            _notificationHandler.Add(new ToastrNotification("Locking out SuperAdmin isn\\'t permitted", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        var authorisedById = _userManager.GetUserId(User);

        var result = await _userManager.LockoutAsync(user.Id, authorisedById, request.EndDate, request.Reason);
        if (result.Succeeded)
        {
            _notificationHandler.Add(new ToastrNotification("Successfully locked out user!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t lockout user", ToastrNotificationType.Error));
            _logger.LogError("Error occured when locking out user", result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLockout(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DisableCurrentLockoutAsync(user.Id);
        if (result.Succeeded)
        {
            _notificationHandler.Add(new ToastrNotification("Successfully removed a user\\'s lockout!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t remove a user\\'s lockout", ToastrNotificationType.Error));
            _logger.LogError("Error occured when removing a user\\'s lockout", result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            _notificationHandler.Add(new ToastrNotification("Couldn\\'t find user to delete", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        if (await _userManager.IsInRoleAsync(user, ApplicationUserRole.SuperAdmin))
        {
            _notificationHandler.Add(new ToastrNotification("SuperAdmin can\\'t be deleted", ToastrNotificationType.Error));
            return RedirectToAction(nameof(Index));
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            _notificationHandler.Add(new ToastrNotification("Successfully deleted user!", ToastrNotificationType.Success));
        }
        else
        {
            _notificationHandler.Add(new ToastrNotification("Internal error, couldn\\'t delete user", ToastrNotificationType.Error));
            _logger.LogError("Error occured deleting user", result.Errors);
        }

        return RedirectToAction(nameof(Index));
    }

    private DateTimeOffset ToClientTime(DateTimeOffset dateTime)
    {
        if (HttpContext.Request.Cookies.ContainsKey("TimeZoneOffset"))
        {
            var offsetString = HttpContext.Request.Cookies["TimeZoneOffset"];
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(offsetString!);

            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        return dateTime;
    }

    private static bool ShouldDisableRoleSelection(IList<string> userRoles, IdentityRole role, ClaimsPrincipal contextUser)
    {
        var roleIsAdminAndContextUserIsNotSuperAdmin = role.Name == ApplicationUserRole.Admin && !contextUser.IsInRole(ApplicationUserRole.SuperAdmin);
        return userRoles.Contains(role.Name) || roleIsAdminAndContextUserIsNotSuperAdmin;
    }
}
