using Aquiles.Communication.Requests.Enderecos;
using Aquiles.Communication.Responses.Enderecos;
using AutoMapper;
using Enderecos.Domain.Entities;

namespace Enderecos.Application.Services.AutoMapper;
public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        EnderecoMap();
    }

    private void EnderecoMap()
    {
        CreateMap<RequestEnderecoJson, Endereco>().ReverseMap();
        CreateMap<Endereco, ResponseEnderecoJson>().ReverseMap();
    }
}
