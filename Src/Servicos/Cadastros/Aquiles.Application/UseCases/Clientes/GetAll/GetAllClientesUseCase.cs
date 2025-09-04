using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Clientes;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
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

    public async Task<PagedResult<ResponseClientesJson>> Execute(int pageNumber, int pageSize, string? search)
    {
        var usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");
        var clientesPaged = await _clienteReadOnlyRepository.GetAll(usuarioId, pageNumber, pageSize, search);

        return new PagedResult<ResponseClientesJson>
        {
            Items = _mapper.Map<IList<ResponseClientesJson>>(clientesPaged.Items),
            TotalCount = clientesPaged.TotalCount
        };
    }
}

