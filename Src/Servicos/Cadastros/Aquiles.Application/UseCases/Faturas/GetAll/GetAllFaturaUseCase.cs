using Aquiles.Application.Servicos.UsuarioLogado;
using Aquiles.Communication.Responses.Faturas;
using Aquiles.Domain.Repositories.Clientes;
using Aquiles.Domain.Repositories.Faturas;
using Aquiles.Domain.Repositories.Planos;
using Aquiles.Exception.AquilesException;
using AutoMapper;

namespace Aquiles.Application.UseCases.Faturas.GetAll;
public class GetAllFaturaUseCase : IGetAllFaturaUseCase
{
    private readonly IMapper _mapper;
    private readonly IFaturaReadOnlyRepository _readOnlyRepository;
    private readonly IUsuarioLogado _usuarioLogado;

    public GetAllFaturaUseCase(
        IMapper mapper,
        IFaturaReadOnlyRepository readOnlyRepository,
        IClienteReadOnlyRepository clienteReadOnlyRepository,
        IPlanoReadOnlyRepository readPlanoOnlyRepository,
        IUsuarioLogado usuarioLogado)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
        _usuarioLogado = usuarioLogado;
    }

    public async Task<IList<ResponseFaturaJson>> Execute()
    {
        var usuarioId = await _usuarioLogado.GetUsuario() ?? throw new InvalidLoginException("Usuário sem permissão");

        var faturas = await _readOnlyRepository.GetAll(usuarioId);
        return _mapper.Map<IList<ResponseFaturaJson>>(faturas);
    }
}
