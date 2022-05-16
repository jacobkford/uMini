namespace uMini.Web.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ShortUrl, UserUrlViewModel>()
            .ForMember(dest => dest.CustomUrl, x => x.MapFrom<AbsoluteShortUrlResolver>());
    }
}
