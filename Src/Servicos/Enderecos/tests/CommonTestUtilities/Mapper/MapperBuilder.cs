using AutoMapper;
using Enderecos.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;
public class MapperBuilder
{
    public static IMapper Build()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig())).CreateMapper();
    }
}
