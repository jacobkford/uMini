using Microsoft.Extensions.Logging;

namespace uMini.Infrastructure.Data;

public class ShortUrlDbContextSeed
{
    public static async Task SeedAsync(ShortUrlDbContext db, ILogger logger)
    {
        try
        {
            if (db.Database.IsSqlServer())
            {
                db.Database.Migrate();
            }

            if (!await db.ShortUrls.AnyAsync())
            {
                await db.ShortUrls.AddAsync(new ShortUrl
                {
                    Key = "google",
                    Url = "https://google.com/",
                    CreatorId = "472963c0-6192-4f48-9006-11761bf99657"
                });

                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}
