namespace uMini.Web.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ShortUrl, ShortUrlViewModel>()
            .ForMember(dest => dest.CustomUrl, x => x.MapFrom<AbsoluteShortUrlViewResolver>());
    }
}
