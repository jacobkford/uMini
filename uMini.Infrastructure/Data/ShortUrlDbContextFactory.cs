using Microsoft.EntityFrameworkCore.Design;
using System.Runtime.InteropServices;

namespace uMini.Infrastructure.Data;

internal class ShortUrlDbContextFactory : IDesignTimeDbContextFactory<ShortUrlDbContext>
{
    public ShortUrlDbContext CreateDbContext(string[] args)
    {
        string shortUrlConnectionString;
        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
        {
            shortUrlConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ShortUrlDatabase")
                ?? throw new ArgumentNullException("Can't find ShortUrlDatabase connection string");
        }
        else
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            shortUrlConnectionString = configuration.GetConnectionString("ShortUrlDatabase");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ShortUrlDbContext>();
        optionsBuilder.UseSqlServer(shortUrlConnectionString);

        return new ShortUrlDbContext(optionsBuilder.Options);
    }
}
