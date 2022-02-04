namespace uMini.Infrastructure.Repositories;

internal class ShortUrlRepository : IShortUrlRepository
{
    private readonly ShortUrlDbContext _context;

    public ShortUrlRepository(ShortUrlDbContext context)
    {
        _context = context;
    }

    public async Task Add(ShortUrl shortUrl)
    {
        await _context.ShortUrls.AddAsync(shortUrl);
    }

    public async Task<ShortUrl?> FindAsync(string key)
    {
        return await _context.ShortUrls.SingleOrDefaultAsync(x => x.Key == key);
    }

    public async Task<IEnumerable<ShortUrl>> FindAllByUserIdAsync(string userId)
    {
        return await _context.ShortUrls
            .Where(x => x.CreatorId == userId)
            .ToListAsync();
    }

    public void Update(ShortUrl shortUrl)
    {
        _context.Entry(shortUrl).State = EntityState.Modified;
    }


    public void Delete(ShortUrl shortUrl)
    {
        _context.Remove(shortUrl);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
