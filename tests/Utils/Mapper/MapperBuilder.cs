using AutoMapper;
using Aquiles.Application.Servicos;

namespace CommonTestUtilities.Mapper;
public class MapperBuilder
{
    public static IMapper Build()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig())).CreateMapper();
    }
}
