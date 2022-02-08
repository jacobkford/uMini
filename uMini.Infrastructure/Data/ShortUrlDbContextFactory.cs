using Microsoft.EntityFrameworkCore.Design;

namespace uMini.Infrastructure.Data;

internal class ShortUrlDbContextFactory : IDesignTimeDbContextFactory<ShortUrlDbContext>
{
    public ShortUrlDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var shortUrlConnectionString = configuration.GetConnectionString("ShortUrlDatabase");

        var optionsBuilder = new DbContextOptionsBuilder<ShortUrlDbContext>();
        optionsBuilder.UseSqlServer(shortUrlConnectionString);

        return new ShortUrlDbContext(optionsBuilder.Options);
    }
}
