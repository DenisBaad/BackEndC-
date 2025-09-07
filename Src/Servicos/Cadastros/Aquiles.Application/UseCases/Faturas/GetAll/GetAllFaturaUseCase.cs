using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Faturas;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Faturas.GetAll;
public class GetAllFaturaUseCase : IGetAllFaturaUseCase
{
    private readonly IMapper _mapper;
    private readonly IFaturaReadOnlyRepository _readOnlyRepository;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly ILogger<GetAllFaturaUseCase> _logger;

    public GetAllFaturaUseCase(
        IMapper mapper,
        IFaturaReadOnlyRepository readOnlyRepository,
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IPlanoReadOnlyRepository readPlanoOnlyRepository,
        IUsuarioLogado usuarioLogado,
        ILogger<GetAllFaturaUseCase> logger)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _usuarioLogado = usuarioLogado;
        _logger = logger;
    }

    public async Task<PagedResult<ResponseFaturaJson>> Execute(int pageNumber, int pageSize)
    {
        Guid? usuarioId = null;

        try
        {
            usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");
            var faturasPaged = await _readOnlyRepository.GetAll(usuarioId.Value, pageNumber, pageSize);

            return new PagedResult<ResponseFaturaJson>
            {
                Items = _mapper.Map<IList<ResponseFaturaJson>>(faturasPaged.Items),
                TotalCount = faturasPaged.TotalCount
            };
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar faturas com usuarioId: {usuarioId}", usuarioId);
            throw;
        }
    }
}
