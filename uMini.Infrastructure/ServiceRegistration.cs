namespace uMini.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityDatabase");
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

        return services;
    }
}
