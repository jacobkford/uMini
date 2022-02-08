namespace uMini.Web.Helpers;

public class AbsoluteShortUrlViewResolver : IValueResolver<ShortUrl, ShortUrlViewModel, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AbsoluteShortUrlViewResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Resolve(ShortUrl source, ShortUrlViewModel destination, string destMember, ResolutionContext context)
    {
        var urlScheme = _httpContextAccessor.HttpContext?.Request.Scheme;
        var urlHost = _httpContextAccessor.HttpContext?.Request.Host;
        var urlPathBase = _httpContextAccessor.HttpContext?.Request.PathBase;

        return $"{urlScheme}://{urlHost}{urlPathBase}/{source.Key}";
    }
}
