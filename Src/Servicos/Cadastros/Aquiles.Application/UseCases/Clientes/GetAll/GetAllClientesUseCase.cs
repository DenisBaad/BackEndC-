using Aquiles.Application.Servicos.UsuarioLogado;
using Aquiles.Communication.Responses.Clientes;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Exception.AquilesException;
using AutoMapper;

namespace Aquiles.Application.UseCases.Clientes.GetAll;
public class GetAllClientesUseCase : IGetAllClientesUseCase
{
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUsuarioLogado _usuarioLogado;

    public GetAllClientesUseCase(
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IMapper mapper,
        IUsuarioLogado usuarioLogado)
    {
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _mapper = mapper;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<IList<ResponseClientesJson>> Execute()
    {
        var usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

        var cliente = await _clienteReadOnlyRepository.GetAll(usuarioId);
        return _mapper.Map<IList<ResponseClientesJson>>(cliente);
    }
}

