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
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies(o => { });

        services.AddScoped<IShortUrlRepository, ShortUrlRepository>();

        services.AddTransient<IEmailSender, AuthenticationMessageSender>();
        services.AddTransient<ISmsSender, AuthenticationMessageSender>();

        return services;
    }
}
