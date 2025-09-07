using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Planos;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Planos.GetAll;
public class GetAllPlanoUseCase : IGetAllPlanoUseCase
{
    private readonly IMapper _mapper;
    private readonly IPlanoReadOnlyRepository _readOnlyRepository;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly ILogger<GetAllPlanoUseCase> _logger;

    public GetAllPlanoUseCase(
        IMapper mapper, 
        IPlanoReadOnlyRepository readOnlyRepository,
        IUsuarioLogado usuarioLogado,
        ILogger<GetAllPlanoUseCase> logger)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _usuarioLogado = usuarioLogado;
        _logger = logger;
    }

    public async Task<PagedResult<ResponsePlanoJson>> Execute(int pageNumber, int pageSize, string? search)
    {
        Guid? usuarioId = null;

        try
        {
            usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");
            var planosPaged = await _readOnlyRepository.GetAll(usuarioId.Value, pageNumber, pageSize, search);

            return new PagedResult<ResponsePlanoJson>
            {
                Items = _mapper.Map<IList<ResponsePlanoJson>>(planosPaged.Items),
                TotalCount = planosPaged.TotalCount
            };
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar planos com usuarioId: {usuarioId}", usuarioId);
            throw;
        }
    }
}
