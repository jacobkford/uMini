namespace uMini.Web.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ShortUrl, ShortUrlViewModel>();
    }
}
