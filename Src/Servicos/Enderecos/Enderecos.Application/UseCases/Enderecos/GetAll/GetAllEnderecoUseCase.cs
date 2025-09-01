using Aquiles.Communication.Responses.Enderecos;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Enderecos.Domain.Repositories.Enderecos;

namespace Enderecos.Application.UseCases.Enderecos.GetAll;
public class GetAllEnderecoUseCase : IGetAllEnderecoUseCase
{
    private readonly IMapper _mapper;
    private readonly IEnderecoReadOnlyRepository _readOnlyRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public GetAllEnderecoUseCase(
        IMapper mapper,
        IEnderecoReadOnlyRepository readOnlyRepository,
        IUsuarioLogado usuarioLogado)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<IList<ResponseEnderecoJson>> Execute()
    {
        var usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

        var endereco = await _readOnlyRepository.GetAll(usuarioId);
        return _mapper.Map<IList<ResponseEnderecoJson>>(endereco);
    }
}
