namespace uMini.Web.Helpers;

public class AbsoluteShortUrlResolver : IValueResolver<ShortUrl, UserUrlViewModel, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AbsoluteShortUrlResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Resolve(ShortUrl source, UserUrlViewModel destination, string destMember, ResolutionContext context)
    {
        var urlScheme = _httpContextAccessor.HttpContext?.Request.Scheme;
        var urlHost = _httpContextAccessor.HttpContext?.Request.Host;
        var urlPathBase = _httpContextAccessor.HttpContext?.Request.PathBase;

        return $"{urlScheme}://{urlHost}{urlPathBase}/{source.Key}";
    }
}
