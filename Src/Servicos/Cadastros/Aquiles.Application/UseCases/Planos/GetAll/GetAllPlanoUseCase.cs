using Aquiles.Communication.Responses;
using Aquiles.Communication.Responses.Planos;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception.AquilesException;
using Aquiles.Utils.UsuarioLogado;
using AutoMapper;

namespace Aquiles.Application.UseCases.Planos.GetAll;
public class GetAllPlanoUseCase : IGetAllPlanoUseCase
{
    private readonly IMapper _mapper;
    private readonly IPlanoReadOnlyRepository _readOnlyRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public GetAllPlanoUseCase(
        IMapper mapper, 
        IPlanoReadOnlyRepository readOnlyRepository,
        IUsuarioLogado usuarioLogado)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<PagedResult<ResponsePlanoJson>> Execute(int pageNumber, int pageSize, string? search)
    {
        var usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");
        var planosPaged = await _readOnlyRepository.GetAll(usuarioId, pageNumber, pageSize, search);

        return new PagedResult<ResponsePlanoJson>
        {
            Items = _mapper.Map<IList<ResponsePlanoJson>>(planosPaged.Items),
            TotalCount = planosPaged.TotalCount
        };
    }
}
