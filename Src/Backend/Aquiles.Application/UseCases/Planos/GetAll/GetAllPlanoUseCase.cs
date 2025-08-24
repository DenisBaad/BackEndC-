using Aquiles.Application.Servicos.UsuarioLogado;
using Aquiles.Communication.Responses.Planos;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception.AquilesException;
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

    public async Task<IList<ResponsePlanoJson>> Execute()
    {
        var usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

        var plano = await _readOnlyRepository.GetAll(usuarioId);
        return _mapper.Map<IList<ResponsePlanoJson>>(plano);
    }
}
