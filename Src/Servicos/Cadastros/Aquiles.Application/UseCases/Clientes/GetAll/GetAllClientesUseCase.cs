using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Clientes;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Clientes.GetAll;
public class GetAllClientesUseCase : IGetAllClientesUseCase
{
    private readonly IClienteReadOnlyRepository _clienteReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly ILogger<GetAllClientesUseCase> _logger;

    public GetAllClientesUseCase(
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IMapper mapper,
        IUsuarioLogado usuarioLogado,
        ILogger<GetAllClientesUseCase> logger)
    {
        _clienteReadOnlyRepository = clienteReadOnlyRepository;
        _mapper = mapper;
        _usuarioLogado = usuarioLogado;
        _logger = logger;
    }

    public async Task<PagedResult<ResponseClientesJson>> Execute(int pageNumber, int pageSize, string? search)
    {
        Guid? usuarioId = null;
        
        try
        {
            usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");
            var clientesPaged = await _clienteReadOnlyRepository.GetAll(usuarioId.Value, pageNumber, pageSize, search);

            return new PagedResult<ResponseClientesJson>
            {
                Items = _mapper.Map<IList<ResponseClientesJson>>(clientesPaged.Items),
                TotalCount = clientesPaged.TotalCount
            };
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar clientes com usuarioId: {usuarioId}", usuarioId);
            throw;
        }
    }
}

