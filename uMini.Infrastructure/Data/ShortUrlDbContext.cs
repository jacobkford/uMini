namespace uMini.Infrastructure.Data;

public class ShortUrlDbContext : DbContext
{
    public DbSet<ShortUrl> ShortUrls { get; set; }

    public ShortUrlDbContext(DbContextOptions<ShortUrlDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
