using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace uMini.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var identityConnectionString = configuration.GetConnectionString("IdentityDatabase");
        var shortUrlConnectionString = configuration.GetConnectionString("ShortUrlDatabase");

        services.AddDbContext<ShortUrlDbContext>(options =>
            options.UseSqlServer(shortUrlConnectionString));

        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(identityConnectionString));

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;

            options.Lockout.AllowedForNewUsers = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
        });

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

        services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(1);

            options.LoginPath = "/i/account/login";
            options.AccessDeniedPath = "/i/account/accessdenied";
            options.LogoutPath = "/i/account/logout";

            options.SlidingExpiration = true;
        });

        services.AddScoped<IShortUrlRepository, ShortUrlRepository>();

        services.AddTransient<IEmailSender, AuthenticationMessageSender>();
        services.AddTransient<ISmsSender, AuthenticationMessageSender>();

        return services;
    }
}
