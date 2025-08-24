using Aquiles.Communication.Responses.Planos;
using Aquiles.Domain.Repositories.Planos;
using AutoMapper;

namespace Aquiles.Application.UseCases.Planos.GetById;
public class GetIdPlanoUseCase : IGetIdPlanoUseCase
{
    private readonly IPlanoReadOnlyRepository _planoReadOnlyRepository;
    private readonly IMapper _mapper;
    
    public GetIdPlanoUseCase(
        IPlanoReadOnlyRepository planoReadOnlyRepository,
        IMapper mapper)
    {
        _planoReadOnlyRepository = planoReadOnlyRepository;
        _mapper = mapper;
    }
    
    public async Task<ResponsePlanoJson> Execute(Guid id)
    {
        var plano = await _planoReadOnlyRepository.GetById(id);
        var response = _mapper.Map<ResponsePlanoJson>(plano);
        return response;
    }
}
