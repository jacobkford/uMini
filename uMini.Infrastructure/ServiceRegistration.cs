using uMini.Infrastructure.Repositories;

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

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

        return services;
    }
}
