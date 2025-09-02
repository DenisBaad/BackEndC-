using Aquiles.Communication.Requests.Clientes;
using Aquiles.Communication.Requests.Faturas;
using Aquiles.Communication.Requests.Planos;
using Aquiles.Communication.Requests.Usuarios;
using Aquiles.Communication.Responses.Clientes;
using Aquiles.Communication.Responses.Faturas;
using Aquiles.Communication.Responses.Planos;
using Aquiles.Communication.Responses.Usuarios;
using Aquiles.Domain.Entities;
using AutoMapper;

namespace Aquiles.Application.Services.AutoMapper;
public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        UsuarioMap();
        ClienteMap();
        CreatePlanoMap();
        CreateFaturaMap();
    }

    private void UsuarioMap()
    {
        CreateMap<RequestCreateUsuariosJson, Usuario>().ReverseMap();
        CreateMap<Usuario, ResponseUsuariosJson>().ReverseMap();
    }

    private void ClienteMap()
    {
        CreateMap<RequestCreateClientesJson, Cliente>().ReverseMap();
        CreateMap<Cliente, ResponseClientesJson>().ReverseMap();
    }

    private void CreatePlanoMap()
    {
        CreateMap<RequestCreatePlanoJson, Plano>().ReverseMap();
        CreateMap<Plano, ResponsePlanoJson>().ReverseMap();
    }
    
    private void CreateFaturaMap()
    {
        CreateMap<RequestCreateFaturaJson, Fatura>().ReverseMap();
        CreateMap<ResponseFaturaJson, Fatura>().ReverseMap();
    }
}
