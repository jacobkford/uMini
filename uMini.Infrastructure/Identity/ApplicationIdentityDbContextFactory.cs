using Microsoft.EntityFrameworkCore.Design;
using System.Runtime.InteropServices;

namespace uMini.Infrastructure.Identity;
internal class ApplicationIdentityDbContextFactory : IDesignTimeDbContextFactory<ApplicationIdentityDbContext>
{
    public ApplicationIdentityDbContext CreateDbContext(string[] args)
    {
        string shortUrlConnectionString;
        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
        {
            shortUrlConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__IdentityDatabase")
                ?? throw new ArgumentNullException("Can't find IdentityDatabase connection string");
        }
        else
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            shortUrlConnectionString = configuration.GetConnectionString("IdentityDatabase");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationIdentityDbContext>();
        optionsBuilder.UseSqlServer(shortUrlConnectionString);

        return new ApplicationIdentityDbContext(optionsBuilder.Options);
    }
}
