namespace uMini.Infrastructure.Repositories;

public interface IShortUrlRepository
{
    Task Add(ShortUrl shortUrl);

    Task<ShortUrl?> FindAsync(string key);

    Task<IEnumerable<ShortUrl>> FindAllByUserIdAsync(string userId);

    void Update(ShortUrl shortUrl);

    void Delete(ShortUrl shortUrl);

    Task Save();
}