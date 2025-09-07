using Aquiles.Communication.Responses.Planos;
using Aquiles.Domain.Repositories.Planos;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aquiles.Application.UseCases.Planos.GetById;
public class GetIdPlanoUseCase : IGetIdPlanoUseCase
{
    private readonly IPlanoReadOnlyRepository _planoReadOnlyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetIdPlanoUseCase> _logger;

    public GetIdPlanoUseCase(
        IPlanoReadOnlyRepository planoReadOnlyRepository,
        IMapper mapper,
        ILogger<GetIdPlanoUseCase> logger)
    {
        _planoReadOnlyRepository = planoReadOnlyRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<ResponsePlanoJson> Execute(Guid id)
    {
        try
        {
            var plano = await _planoReadOnlyRepository.GetById(id);
            var response = _mapper.Map<ResponsePlanoJson>(plano);
            return response;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar planos com planoId: {planoId}", id);
            throw;
        }
    }
}
